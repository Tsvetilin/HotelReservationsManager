using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Threading.Tasks;
using Web;
using Xunit;

namespace Tests.Web.Tests
{
    public class HomeControllerTests
         : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public HomeControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/home/privacy")]
        [InlineData("/rooms")]
        [InlineData("/identity/account/login")]
        [InlineData("home/error")]
        [InlineData("/reservations")]

        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [InlineData("/rooms/create")]
        [InlineData("/rooms/details")]
        [InlineData("/rooms/update")]
        [InlineData("/reservations")]
        [InlineData("/reservations/create")]
        [InlineData("/reservations/details")]
        [InlineData("/reservations/update")]
        [InlineData("/users/add")]
        [InlineData("/users/all")]
        [InlineData("/users/update")]
        [InlineData("/users/promote")]
        public async Task Get_EndpointsReturnRedirectUnauthorizedAndCorrectContentType(string url)
        {
            // Arrange
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions { 
            AllowAutoRedirect=false});

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        }

        [Theory]
        [InlineData("/home/index/-1?pageSize=-1")]

        public async Task GetIndex_ShouldValidateInvalidPageParams(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }

    }
}
