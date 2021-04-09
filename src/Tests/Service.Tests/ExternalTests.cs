using NUnit.Framework;
using Services.External;

namespace Tests.Service.Tests
{
    public class ExternalTests
    {
        [Test]
        public void SendEmail_ShouldNotThrowError()
        {
            var emailSender = new EmailSender("Test Key", "sender email", "sender name");
            Assert.DoesNotThrowAsync(()=> emailSender.SendEmailAsync("random email", "subject", "message"));
        }
    }
}
