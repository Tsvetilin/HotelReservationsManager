using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
    public class Reservation
    {
        public Reservation()
        {
            this.Id = Guid.NewGuid().ToString();

        }
        public string Id { get; set; }
        public ApplicationUser User { get; set; }
        public List<ApplicationUser> Clients { get; set; }
        public DateTime AccommodationDate { get; set; }
        public DateTime ReleaseDate { get; set; }
        public bool Breakfast { get; set; }
        public bool AllInclusive { get; set; }
        public double Price { get; set; }
        public virtual Room Room { get; set; }
    }
}
