using Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace Tests.Common
{
    public class InMemoryFactory
    {
        public static ApplicationDbContext InitializeContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().
                EnableSensitiveDataLogging().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }
    }
}
