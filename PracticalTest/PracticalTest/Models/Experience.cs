using System.ComponentModel.DataAnnotations;

namespace PracticalTest.Models
{
    public class Experience
    {
        [Key]
        public int WorkExperiencesId { get; set; }
        public string Company { get; set; }
        public string Designation { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }

}
