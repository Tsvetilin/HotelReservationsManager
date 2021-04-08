using Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Threading.Tasks;
using Tests.Common;
using Web;
using Xunit;
using static Tests.Common.TestAuthHandler;

namespace Tests.Web.Tests
{
    public class ReservarionsControllerTests : IClassFixture<CustomAppFactory>
    {
        private readonly CustomAppFactory _factory;

        public ReservarionsControllerTests(CustomAppFactory factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/reservations")]
        public async Task GetIndex_ReturnCorrect(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}