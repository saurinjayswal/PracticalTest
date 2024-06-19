using System.ComponentModel.DataAnnotations;

namespace PracticalTest.Models
{
    public class LanguageDetails
    {
        [Key]
        public int LanguagesId { get; set; }
        public string Language { get; set; }
        public bool CanRead { get; set; }
        public bool CanWrite { get; set; }
        public bool CanSpeak { get; set; }
    }

}
