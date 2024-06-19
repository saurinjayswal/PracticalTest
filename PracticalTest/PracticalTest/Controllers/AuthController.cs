using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Text;
using PracticalTest.Context;
using Microsoft.EntityFrameworkCore;
using PracticalTest.Models;
using PracticalTest.Helpers;
using PracticalTest.Models.Dto;

namespace PracticalTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {    
        private readonly ApplicationDbContext _appDbContext;
        public AuthController(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpPost("authentication")]
        public async Task<IActionResult> Authenticate([FromBody] User model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.UserName == model.UserName);
            if (user == null)
            {
                return NotFound(new { Message = "User not Found!" });
            }
            if (!PasswordHasher.VerifyPassword(model.Password, user.Password))
            {
                return BadRequest(new { Message = "Password is incorrect!" });
            }

            user.Token = CreateJwt(user);
            var newAccessToken = user.Token;
            var newRefreshToken = CreateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(5);

            await _appDbContext.SaveChangesAsync();


            return Ok(new TokenApiDto()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] User model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            // check Email
            if (await CheckEmailExistAsync(model.Email))
            {
                return BadRequest(new { Message = "Email Already Exist" });
            }

            //chek UserName
            if (await CheckUserNameExistAsync(model.UserName))
            {
                return BadRequest(new { Message = "UserName Already Exist" });
            }

            //check Password
            var passMessage = CheckPasswordStrength(model.Password);
            if (!string.IsNullOrEmpty(passMessage))
            {
                return BadRequest(new { Message = passMessage.ToString() });
            }

            model.Password = PasswordHasher.HashPassword(model.Password);
            model.Role = "User";
            model.Token = "";
            model.RefreshToken = "";
            await _appDbContext.Users.AddAsync(model);
            await _appDbContext.SaveChangesAsync();

            return Ok(new
            {
                Message = "User Registered!",
            }
            );
        }

        private Task<bool> CheckEmailExistAsync(string? email) => _appDbContext.Users.AnyAsync(x => x.Email == email);

        private Task<bool> CheckUserNameExistAsync(string? username) => _appDbContext.Users.AnyAsync(x => x.UserName == username);

        private static string CheckPasswordStrength(string pass)
        {
            StringBuilder sb = new StringBuilder();
            if (pass.Length < 9)
            {
                sb.Append("Minimum password length should be 8" + Environment.NewLine);
            }
            if (!(Regex.IsMatch(pass, "[a-z]") &&
                  Regex.IsMatch(pass, "[A-Z]") &&
                  Regex.IsMatch(pass, "[0-9]")))
            {
                sb.Append("Password should be AlphaNumeric" + Environment.NewLine);
            }
            if (!Regex.IsMatch(pass, "[<,>,@,!,#,$,%,^,&,*,(,),_,+,\\[,\\],{,},?,:,;,|,',\\,.,/,~,`,-,=]"))
            {
                sb.Append("Password should contain special charcter" + Environment.NewLine);
            }
            return sb.ToString();
        }

        private string CreateJwt(User model)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("This is my very very secret key.....");
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, model.Role),
                new Claim(ClaimTypes.Name, $"{model.UserName}")
            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddSeconds(60),
                SigningCredentials = credentials
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }

        private string CreateRefreshToken()
        {
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var refreshToken = Convert.ToBase64String(tokenBytes);

            var tokenInUser = _appDbContext.Users.Any(x => x.RefreshToken == refreshToken);
            if (tokenInUser)
            {
                return CreateRefreshToken();
            }
            return refreshToken;
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var key = Encoding.ASCII.GetBytes("This is my very very secret key.....");
            var tokenValidationParameter = new TokenValidationParameters()
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = false
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameter, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("This Is Invalid Token");
            }
            return principal;
        }

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin")]
        [HttpGet("getalluser")]
        public async Task<ActionResult<User>> GetAllUser()
        {
            return Ok(await _appDbContext.Users.ToListAsync());
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenApiDto model)
        {
            if (model is null)
            {
                return BadRequest("Invalid client request");
            }
            string accessToken = model.AccessToken;
            string refreshToken = model.RefreshToken;

            var principal = GetPrincipalFromExpiredToken(accessToken);

            var userName = principal.Identity.Name;
            var existedUser = await _appDbContext.Users.FirstOrDefaultAsync(x => x.UserName == userName);
            if (existedUser is null || existedUser.RefreshToken != refreshToken || existedUser.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Invalid Request");
            }

            var newAccessToken = CreateJwt(existedUser);
            var newRefreshToken = CreateRefreshToken();
            existedUser.RefreshToken = newRefreshToken;
            await _appDbContext.SaveChangesAsync();

            return Ok(new TokenApiDto()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }
    }
}
