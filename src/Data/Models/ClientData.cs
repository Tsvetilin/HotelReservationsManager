using System;

namespace Data.Models
{
    /// <summary>
    /// Clients data scheme
    /// </summary>
    public class ClientData
    {
        public ClientData()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool IsAdult { get; set; }

        public virtual Reservation Reservation { get; set; }
        public string ReservationId { get; set; }
    }
}
