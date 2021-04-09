using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Web.Models.Settings
{
    public class SettingsIndexModel
    {
        [Required]
        [DisplayName("Breakfast Price")]
        [Range(0,double.MaxValue)]
        public double BreakfastPrice { get; set; }

        [Required]
        [DisplayName("All-inclusive Price")]
        [Range(0, double.MaxValue)]
        public double AllInclusivePrice { get; set; }

    }
}
