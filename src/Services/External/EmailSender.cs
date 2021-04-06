using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace Services.External
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
        }
    }
}
