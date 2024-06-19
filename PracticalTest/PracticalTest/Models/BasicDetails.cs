using System.ComponentModel.DataAnnotations;

namespace PracticalTest.Models
{
    public class BasicDetails
    {
        [Key]
        public int BasicDetailsId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string Contact { get; set; }
    }
}
