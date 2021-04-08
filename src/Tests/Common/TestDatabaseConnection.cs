using Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Tests.Common
{
    public static class TestDatabaseConnection
    {
        public const string TestingConnectionString = "Server=(localdb)\\mssqllocaldb;Database=HotelManagerTestingDb-{0};Trusted_Connection=True;MultipleActiveResultSets=true";
        public static List<string> ConnectionStrings = new();


        public static string GetConnectionString()
        {
            var connection = string.Format(TestingConnectionString, Guid.NewGuid().ToString());
            ConnectionStrings.Add(connection);
            return connection;
        }

        public static void RemoveTestDatabases()
        {
            foreach (var connection in ConnectionStrings)
            {
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                                    .UseSqlServer(connection)
                                    .Options;

                var context = new ApplicationDbContext(options);

                context.Database.EnsureDeleted();
            }
        }
    }
}
