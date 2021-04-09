using Data.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Web.Models.Rooms
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

        [DisplayName("Upload photo")]
        public IFormFile PhotoUpload { get; set; }

        [DisplayName("Use same photo")]
        public bool UseSamePhoto { get; set; }
    }
}
