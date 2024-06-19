using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PracticalTest.Models
{
    public class ApplicationForm
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("BasicDetailsId")]
        public BasicDetails BasicDetails { get; set; }
        [ForeignKey("EducationDetailsId")]
        public Education EducationDetails { get; set; }
        [ForeignKey("WorkExperiencesId")]
        public Experience WorkExperiences { get; set; }
        [ForeignKey("LanguagesId")]
        public LanguageDetails KnownLanguages { get; set; }
        [ForeignKey("TechnicalExperiencesId")]
        public TechExpertise TechnicalExperiences { get; set; }
        [ForeignKey("PreferenceId")]
        public Preference Preference { get; set; }
    }
}
