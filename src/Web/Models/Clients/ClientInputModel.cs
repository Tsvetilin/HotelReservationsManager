using System.ComponentModel.DataAnnotations;

namespace Web.Models.Clients
{
    public class ClientInputModel
    {
        public string Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }
        public bool IsAdult { get; set; }
    }
}
