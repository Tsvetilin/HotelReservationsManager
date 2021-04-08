using Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Tests.Common;
using Web;
using Xunit;
using Xunit.Extensions.Ordering;

//[assembly: TestFramework("AssemblyFixtureExample.XunitExtensions.XunitTestFrameworkWithAssemblyFixture", "AssemblyFixtureExample")]
//[assembly: AssemblyFixture(typeof(MyAssemblyFixture))]


namespace Tests.Web.Tests
{
    [CollectionDefinition("HomeTestsUnauthorized", DisableParallelization = true)]
    public class HomeTestsUnauthorized
         : IClassFixture<WebApplicationFactory<Startup>>, IClassFixture<DisposableClassFixture>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public HomeTestsUnauthorized(WebApplicationFactory<Startup> factory)
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
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var db = services.SingleOrDefault(
                             d => d.ServiceType ==
                        typeof(ApplicationDbContext));

                    var dbo = services.SingleOrDefault(
                         d => d.ServiceType ==
                    typeof(DbContextOptions<ApplicationDbContext>));

                    services.Remove(db);
                    services.Remove(dbo);

                    services.AddDbContext<ApplicationDbContext>((options, context) =>
                    {
                        //Safe Connection string, but memory intense ->TestDatabaseConnectionProvider.GetConnectionStringDisposable()
                        //Unsage Connection string -> TestDatabaseConnectionProvider.Instance.SharedConnectionStringDisposable
                        context.UseSqlServer(TestDatabaseConnectionProvider.Instance.SharedConnectionStringDisposable);
                    });
                });
            }).CreateClient();

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
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var db = services.SingleOrDefault(
                             d => d.ServiceType ==
                        typeof(ApplicationDbContext));

                    var dbo = services.SingleOrDefault(
                         d => d.ServiceType ==
                    typeof(DbContextOptions<ApplicationDbContext>));

                    services.Remove(db);
                    services.Remove(dbo);

                    services.AddDbContext<ApplicationDbContext>((options, context) =>
                    {
                        //Safe Connection string, but memory intense ->TestDatabaseConnectionProvider.GetConnectionStringDisposable()
                        //Unsage Connection string -> TestDatabaseConnectionProvider.Instance.SharedConnectionStringDisposable
                        context.UseSqlServer(TestDatabaseConnectionProvider.Instance.SharedConnectionStringDisposable);
                    });
                });
            }).CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

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
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var db = services.SingleOrDefault(
                             d => d.ServiceType ==
                        typeof(ApplicationDbContext));

                    var dbo = services.SingleOrDefault(
                         d => d.ServiceType ==
                    typeof(DbContextOptions<ApplicationDbContext>));

                    services.Remove(db);
                    services.Remove(dbo);

                    services.AddDbContext<ApplicationDbContext>((options, context) =>
                    {
                        //Safe Connection string, but memory intense ->TestDatabaseConnectionProvider.GetConnectionStringDisposable()
                        //Unsage Connection string -> TestDatabaseConnectionProvider.Instance.SharedConnectionStringDisposable
                        context.UseSqlServer(TestDatabaseConnectionProvider.Instance.SharedConnectionStringDisposable);
                    });
                });
            }).CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }

    }
}
