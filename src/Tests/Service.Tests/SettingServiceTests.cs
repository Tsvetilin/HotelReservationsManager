using Data;
using Data.Models;
using NUnit.Framework;
using Services.Data;
using System.Collections.Generic;
using System.Linq;
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
            Assert.AreEqual(Settings.Breakfast.Key, context.Settings.First(x => x.Value == result.Value).Key);
        }

        [Test]
        public async Task AddSetting_ShouldAddSetting()
        {
            // Arange
            ApplicationDbContext context = InMemoryFactory.InitializeContext();

            var service = new SettingService(context);
            var initialCount = context.Settings.Count();

            // Act
            await service.AddAsync("Key", "Value", "string");

            // Assert
            Assert.AreEqual(initialCount + 1, context.Settings.Count());
        }

        [Test]
        public async Task UpdateSetting_ShouldUpdateSetting()
        {
            // Arange
            ApplicationDbContext context = InMemoryFactory.InitializeContext();

            var service = new SettingService(context);
            var initialCount = context.Settings.Count();

            // Act
            await service.UpdateAsync("Key", "Value1", "string");

            // Assert
            Assert.AreEqual(initialCount, context.Settings.Count());
        }
    }
}
