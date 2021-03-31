using System;

namespace Data.Models
{
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

    }
}
