using Services.Mapping;
using System.Net;
using System.Threading.Tasks;
using Tests.Common;
using Web.Models;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true, MaxParallelThreads = 1)]
namespace Tests.Web.Tests
{
    [CollectionDefinition("ReservarionsControllerTests", DisableParallelization = true)]
    public class ReservarionsControllerTests : IClassFixture<CustomAppFactory>, IClassFixture<DisposableClassFixture>
    {
        private readonly CustomAppFactory _factory;

        public ReservarionsControllerTests(CustomAppFactory factory)
        {
            _factory = factory;
            MappingConfig.RegisterMappings(typeof(ErrorViewModel).Assembly);
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