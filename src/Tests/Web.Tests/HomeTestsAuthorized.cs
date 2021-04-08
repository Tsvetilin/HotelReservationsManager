using Tests.Common;
using Xunit;
using System.Net.Http;
using System.Net;
using Xunit.Extensions.Ordering;

namespace Tests.Web.Tests
{
    [CollectionDefinition("HomeTestsAuthorized", DisableParallelization = true)]
    public class HomeTestsAuthorized : IClassFixture<CustomAppFactory>, IClassFixture<DisposableClassFixture>
    {
        private readonly CustomAppFactory factory;
        private readonly HttpClient client;

        public HomeTestsAuthorized(CustomAppFactory factory)
        {
            this.factory = factory;
            this.client = factory.WithWebHostBuilder(builder =>
            {

            }).CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Theory]
        [InlineData("/rooms/create")]
        [InlineData("/reservations")]
        [InlineData("/users/add")]
        [InlineData("/users/all")]
        [InlineData("/identity/account/manage")]
        [InlineData("/identity/account/login")]
        [InlineData("/identity/account/forgotpassword")]
        [InlineData("/identity/account/manage/personaldata")]
        [InlineData("/identity/account/manage/setpassword")]

        public async void Get_ShouldReturnPage(string url)
        {
            var res = await client.GetAsync(url);

            Assert.Equal(HttpStatusCode.OK, res.StatusCode);
        }

        [Theory]
        //[InlineData("/rooms/details")]
        [InlineData("/rooms/update")]
        // [InlineData("/reservations")]
        [InlineData("/reservations/create")]
        [InlineData("/reservations/details")]
        [InlineData("/reservations/update")]
        //[InlineData("/users/add")]
        // [InlineData("/users/all")]
        //  [InlineData("/users/update")]
        //  [InlineData("/users/promote")]

        public async void Get_ShouldReturnNotFound(string url)
        {
            var res = await client.GetAsync(url);

            Assert.Equal(HttpStatusCode.NotFound, res.StatusCode);
        }

        [Theory]
        //  [InlineData("/rooms/details")]
        //  [InlineData("/rooms/update")]
        [InlineData("/reservations/create")]
        [InlineData("/reservations/details")]
        [InlineData("/reservations/update")]
        // [InlineData("/users/add")]
        // [InlineData("/users/all")]
        // [InlineData("/users/update")]
        // [InlineData("/users/promote")]

        public async void Post_ShouldReturnNotFound(string url)
        {
            var res = await client.PostAsync(url, null);

            Assert.Equal(HttpStatusCode.NotFound, res.StatusCode);
        }


        [Theory]
        [InlineData("/rooms/update")]
        [InlineData("/users/all")]
        [InlineData("/users/update")]
        [InlineData("/users/promote")]

        public async void Post_ShouldReturnOk(string url)
        {
            var res = await client.PostAsync(url, null);

            Assert.Equal(HttpStatusCode.OK, res.StatusCode);
        }
    }
}
