using System.ComponentModel;

namespace Web.Models.Clients
{
    public class ClientInputModel
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        [DisplayName("Is Adult")]
        public bool IsAdult { get; set; }
    }
}
