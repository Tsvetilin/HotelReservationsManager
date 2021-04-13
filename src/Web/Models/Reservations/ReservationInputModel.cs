using Data.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Web.Models.Clients;
using Web.Models.Rooms;

namespace Web.Models.Reservations
{
    public class ReservationInputModel
    {
        public string Id { get; set; }

        public IList<ClientInputModel> Clients { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [DisplayName("Accomodation date")]
        public DateTime AccommodationDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Release date")]
        public DateTime ReleaseDate { get; set; }

        public bool Breakfast { get; set; }

        public bool AllInclusive { get; set; }

        [BindNever]
        public double BreakfastPrice { get; set; }

        [BindNever]
        public double AllInclusivePrice { get; set; }

        [BindNever]
        public double Price { get; set; }

        [BindNever]
        public string RoomId { get; set; }

        [BindNever]
        public IEnumerable<ReservationPeriod> Reservations { get; set; }

        [BindNever]
        public int RoomCapacity { get; set; }

        [BindNever]
        public RoomType RoomType { get; set; }

        [BindNever]
        public double RoomAdultPrice { get; set; }

        [BindNever]
        public double RoomChildrenPrice { get; set; }

        [BindNever]
        public string RoomImageUrl { get; set; }

        public string UserId { get; set; }

        public string GetPeriods()
        {
            StringBuilder sb = new();
            sb.Append('[');
            if (Reservations?.Any() ?? false)
            {
                foreach (var period in Reservations)
                {
                    sb.Append($@"{{start: new Date(""{period.AccommodationDate:yyyy-MM-dd}""), end: new Date(""{period.ReleaseDate:yyyy-MM-dd}"")}},");
                }
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append(']');
            return sb.ToString();
        }
    }
}
