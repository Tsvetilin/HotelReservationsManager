using Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Tests.Common
{
    /// <summary>
    /// Unique dummy test database connection strings manager
    /// </summary>
    public sealed class TestDatabaseConnectionProvider
    {
        public const string TestingConnectionStringTemplate = "Server=(localdb)\\mssqllocaldb;Database=HotelManagerTestingDb_{0};Trusted_Connection=True;MultipleActiveResultSets=true";
        private static List<string> ConnectionStrings = new();

        private static readonly TestDatabaseConnectionProvider instance = new TestDatabaseConnectionProvider();
        public string SharedConnectionStringDisposable;

        static TestDatabaseConnectionProvider()
        {
        }

        private TestDatabaseConnectionProvider()
        {
            SharedConnectionStringDisposable = GetConnectionStringDisposable();
        }

        public static TestDatabaseConnectionProvider Instance
        {
            get
            {
                return instance;
            }
        }

        public static string GetConnectionStringDisposable()
        {
            var connection = GetConnectionString();
            ConnectionStrings.Add(connection);
            return connection;
        }

        public static string GetConnectionString()
        {
            return string.Format(TestingConnectionStringTemplate, Guid.NewGuid().ToString()); ;
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
