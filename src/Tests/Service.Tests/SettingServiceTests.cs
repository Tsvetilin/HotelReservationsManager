using Data;
using Data.Models;
using NUnit.Framework;
using Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Common;
using Tests.Data;

namespace Tests.Service.Tests
{
    class SettingServiceTests
    {
        [Test]
        public async Task GetSetting_ShouldReturnSetting()
        {
            // Arange
            List<Setting> settings = new()
            {
                Settings.Breakfast,
                Settings.AllInclusive
            };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(settings);
            var service = new SettingService(context);

            // Act
            var result = await service.GetAsync(Settings.Breakfast.Key);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(Settings.Breakfast.Value, result.Value);
            Assert.AreEqual(Settings.Breakfast.Type, result.Type);
            Assert.AreEqual(Settings.Breakfast.Key, context.Settings.First(x=>x.Value==result.Value).Key);
        }
    }
}
