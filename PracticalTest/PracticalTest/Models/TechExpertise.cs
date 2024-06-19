using System.ComponentModel.DataAnnotations;

namespace PracticalTest.Models
{
    public class TechExpertise
    {
        [Key]
        public int TechnicalExperiencesId { get; set; }
        public string Technology { get; set; }
        public string ProficiencyLevel { get; set; }
    }
}
