using Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace Web.Models.InputModels
{
    public class RoomInputModel
    {
      
        [Required]
        [Range(1, 10, ErrorMessage = "The capacity should be between {0} and {1}")]

        public int Capacity { get; set; }
        [Required]
        public RoomType Type { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public double AdultPrice { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public double ChildrenPrice { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public int Number { get; set; }

    }
}
