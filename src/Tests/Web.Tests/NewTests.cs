using Data;
using Tests.Common;
using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System;

namespace Tests.Web.Tests
{
    public class NewTests : IClassFixture<CustomAppFactory>
    {
        private readonly CustomAppFactory factory;
        private readonly HttpClient client;

        public NewTests(CustomAppFactory factory)
        {
            this.factory = factory;
            this.client = factory.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
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
        [InlineData("/identity/account/changepassword")]
        public async void Testinggg(string url)
        {
            var res = await client.GetAsync(url);

            Assert.Equal(System.Net.HttpStatusCode.OK, res.StatusCode);
        }
    }
}
