using PracticalTest.Models;
using System.ComponentModel.DataAnnotations;

namespace PracticalTest.Models
{
    public class Preference
    {
        [Key]
        public int PreferenceId { get; set; }
        public string PreferredLocation { get; set; }
        public string ExpectedCTC { get; set; }
        public string CurrentCTC { get; set; }
        public int NoticePeriod { get; set; }
    }
}


