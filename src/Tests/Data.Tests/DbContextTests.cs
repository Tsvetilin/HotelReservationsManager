using Data;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Xunit;

namespace Tests.Data.Tests
{
    public class DbContextTests
    {
        [Fact]
        public void InitializeDbTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().
                UseSqlServer(TestDatabaseConnection.TestingConnectionString).Options;

            var context = new ApplicationDbContext(options);

            var exception = Record.Exception(() => context.Database.Migrate());

            Assert.Null(exception);
            
            context.Database.EnsureDeleted();
        }
    }
}
