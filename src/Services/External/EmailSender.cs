using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

/// <summary>
/// Controller layer related to external services logic
/// </summary>
namespace Services.External
{
    /// <summary>
    /// SendGrid email sender implementation
    /// </summary>
    public class EmailSender : IEmailSender
    {
        private readonly SendGridClient client;
        private readonly string sender;
        private readonly string senderName;

        public EmailSender(string APIKey, string sender, string senderName)
        {
            this.client = new SendGridClient(APIKey);
            this.sender = sender;
            this.senderName = senderName;
        }

        /// <summary>
        /// Sends email to a selected receiver with subject and message
        /// </summary>
        /// <param name="email">Email of the reciever</param>
        /// <param name="subject">Subject of the email</param>
        /// <param name="htmlMessage">Message to send</param>

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var from = new EmailAddress(sender, senderName);
            var to = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, null, htmlMessage);
            await client.SendEmailAsync(msg);
        }
    }
}
