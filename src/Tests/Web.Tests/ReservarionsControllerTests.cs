using Data;
using Data.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Services;
using Services.Data;
using System.Net;
using System.Threading.Tasks;
using Tests.Common;
using Web;
using Xunit;

namespace Tests.Web.Tests
{
    public class ReservarionsControllerTests : IClassFixture<CustomWebAppFactory<Startup>>
    {
        private readonly CustomWebAppFactory<Startup> _factory;

        public ReservarionsControllerTests(CustomWebAppFactory<Startup> factory)
        {
            _factory = factory;
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
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                HandleCookies = true,
                MaxAutomaticRedirections = 7,
            });

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        }

        [Theory]
        [InlineData("/reservations")]
        public async Task GetIndex_ReturnCorrect(string url)
        {
            // Arrange
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddDbContext<ApplicationDbContext>((options, context) =>
                    {
                        context.UseInMemoryDatabase("TestingDb");
                    });
                    services.AddTransient<ISettingService, SettingService>();
                    services.AddTransient<IUserService, UserService>();
                    services.AddTransient<IMemoryCache, MemoryCache>();
                    services.AddSingleton<AuthenticationHandler<AuthenticationSchemeOptions>, TestAuthHandler>();
                });
            }).CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
