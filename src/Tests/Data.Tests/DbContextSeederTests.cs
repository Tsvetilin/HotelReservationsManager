using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Tests.Common;
using Xunit;

/// <summary>
/// Tests of the Model layer project
/// </summary>
namespace Tests.Data.Tests
{
    public class DbContextSeederTests
    {
        [Fact]
        public async Task SeederTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().
                UseSqlServer(TestDatabaseConnectionProvider.GetConnectionStringDisposable()).Options;

            var context = new ApplicationDbContext(options);
            context.Database.Migrate();

            var seeder = new ApplicationDbContextSeeder();

            var logMock = new Mock<ILogger>();

            var exception = await Record.ExceptionAsync(() => seeder.SeedAsync(context,logMock.Object));

            Assert.Null(exception);

            context.Database.EnsureDeleted();
        }

    }
}
