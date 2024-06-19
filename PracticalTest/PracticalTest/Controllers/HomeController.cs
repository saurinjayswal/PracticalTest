using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracticalTest.Context;
using PracticalTest.Models;

namespace PracticalTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IList<ApplicationForm>>> GetApplications()
        {
            return await _context.applicationForms.
                 Include(a => a.BasicDetails)
                .Include(a => a.EducationDetails)
                .Include(a => a.WorkExperiences)
                .Include(a => a.KnownLanguages)
                .Include(a => a.TechnicalExperiences)
                .Include(a => a.Preference).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationForm>> GetApplicationById(int id)
        {
          /*  var application = await _context.applicationForms.FindAsync(id);*/
            var application = await _context.applicationForms
                .Include(a => a.BasicDetails)
                .Include(a => a.EducationDetails)
                .Include(a => a.WorkExperiences)
                .Include(a => a.KnownLanguages)
                .Include(a => a.TechnicalExperiences)
                .Include(a => a.Preference)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (application == null)
            {
                return NotFound();
            }

            return application;
        }

        [HttpPost]
        public async Task<ActionResult<ApplicationForm>> AddApplication(ApplicationForm application)
        {
            _context.applicationForms.Add(application);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetApplicationById), new { id = application.Id }, application);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutApplication(int id, ApplicationForm application)
        {
            if (id != application.Id)
            {
                return BadRequest();
            }

            _context.Entry(application).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }            

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApplication(int id)
        {
            var application = await _context.applicationForms
                .Include(a => a.BasicDetails)
                .Include(a => a.EducationDetails)
                .Include(a => a.WorkExperiences)
                .Include(a => a.KnownLanguages)
                .Include(a => a.TechnicalExperiences)
                .Include(a => a.Preference)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (application == null)
            {
                return NotFound();
            }

            _context.applicationForms.Remove(application);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ApplicationExists(int id)
        {
            return _context.applicationForms.Any(e => e.Id == id);
        }
    }
}
