using NUnit.Framework;
using Services.External;
using System.IO;

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

        [Test]
        public void UploadImage_ShouldNotThrowError()
        {
            var ImageManager = new ImageManager("cloud Name", "api key", "api sender");
            Assert.DoesNotThrowAsync(() => ImageManager.UploadImageAsync(new MemoryStream(),"file name"));
        }
    }
}
