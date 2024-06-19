using System.ComponentModel.DataAnnotations;

namespace PracticalTest.Models
{
    public class Education
    {
        [Key]
        public int EducationDetailsId { get; set; }
        public string Level { get; set; }
        public string BoardUniversity { get; set; }
        public int Year { get; set; }
        public double CGPAOrPercentage { get; set; }
    }

}
