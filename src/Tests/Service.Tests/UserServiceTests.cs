using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Services;
using Services.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tests.Common;
using Tests.Data;
using Web.Models;
using Web.Models.ViewModels;

namespace Tests.Service.Tests
{
    public class UserServiceTests
    {

        [SetUp]
        public void Setup()
        {
            MappingConfig.RegisterMappings(typeof(ErrorViewModel).Assembly);
        }

        [Test]
        public async Task GetEmployeeAsync_ShouldFindEmployee()
        {
            // Arange
            List<ApplicationUser> userData = new() { Users.User1Employee };
            List<EmployeeData> employeeData = new() { Users.EmployeeUser1 };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(userData)
                                                                .SeedAsync(employeeData);
            var service = new UserService(context);

            // Act
            var result = await service.GetEmployeeAsync<EmployeeDataViewModel>(employeeData.First().UserId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(employeeData.First().UserId,result.UserId);
        }

        [Test]
        public async Task GetUserAsync_ShouldFindUser()
        {
            // Arange
            List<ApplicationUser> userData = new() { Users.User1Employee };
            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(userData);
            var service = new UserService(context);

            // Act
            var result = await service.GetUserAsync<UserDataViewModel>(userData.First().Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual( userData.First().Id,result.Id);
        }

        [Test]
        public async Task AddAsync_ShouldAddEmployee()
        {
            // Arange
            List<ApplicationUser> userData = new() { Users.User1Employee , Users.User2Employee};
            List<EmployeeData> employeeData = new() { Users.EmployeeUser1 };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(userData)
                                                                .SeedAsync(employeeData);
            var service = new UserService(context);

            // Act
            await service.AddAsync(Users.EmployeeUser2);

            
            // Assert
            Assert.AreEqual(employeeData.Count+1, context.Users.Count());
        }

        [Test]
        public async Task GetAllEmployeesAsync_ShouldReturnAllEmployees()
        {
            // Arange
            List<EmployeeData> employeeData = new() { Users.EmployeeUser1, Users.EmployeeUser2 };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(employeeData);
            var service = new UserService(context);

            // Act
            var result = await service.GetAllEmployees<EmployeeDataViewModel>();

            // Assert
            Assert.AreEqual(employeeData.Count(), result.Count());
        }

        [Test]
        public async Task GetAllUsersAsync_ShouldReturnAllUsers()
        {
            // Arange
            List<ApplicationUser> usersData = new() { Users.User3NotEmployee, Users.User4NotEmployee };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(usersData);
            var service = new UserService(context);

            // Act
            var result = await service.GetAllUsers<UserDataViewModel>();

            // Assert
            Assert.AreEqual(usersData.Count(), result.Count());
        }

        [Test]
        public async Task GetEmployeesSearchResults_ShouldFindExistingSearchedResult()
        {
            // Arange
            List<ApplicationUser> userData = new() { Users.UserForSearch };
            List<EmployeeData> employeeData = new() { Users.EmployeeForSearch };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(userData)
                                                                .SeedAsync(employeeData);
            var service = new UserService(context);

            // Act
            var result = await service.GetEmployeesSearchResults<EmployeeDataViewModel>(Users.searchParam);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public async Task GetUsersSearchResults_ShouldFindExistingSearchedResult()
        {
            // Arange
            List<ApplicationUser> userData = new() { Users.UserForSearch };
            List<EmployeeData> employeeData = new() { Users.EmployeeForSearch };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(userData)
                                                                .SeedAsync(employeeData);
            var service = new UserService(context);

            // Act
            var result = await service.GetUsersSearchResults<UserDataViewModel>(Users.searchParam);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public async Task CountAllUsers_ShouldCountAllUsers()
        {
            // Arange
            List<ApplicationUser> usersData = new() { Users.User3NotEmployee, Users.User4NotEmployee };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(usersData);
            var service = new UserService(context);

            // Act
            var result = service.CountAllUsers();

            // Assert
            Assert.AreEqual(usersData.Count(), result);
        }

        [Test]
        public async Task CountAllEmployees_ShouldCountAllemployees()
        {
            // Arange
            List<EmployeeData> employeeData = new() { Users.EmployeeUser1, Users.EmployeeUser2 };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(employeeData);
            var service = new UserService(context);

            // Act
            var result = service.CountAllEmployees();

            // Assert
            Assert.AreEqual(employeeData.Count(), result);
        }
        

            [Test]
        public async Task IsAlreadyAdded_ShouldFindAddeUser()
        {
            // Arange
            List<ApplicationUser> usersData = new() { Users.UserForSearch };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(usersData);
            var service = new UserService(context);

            // Act
            var result = service.IsAlreadyAdded(Users.searchParam);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateEmployeeData()
        {
            // Arange
            List<EmployeeData> employeeData = new() { Users.EmployeeForSearch };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(employeeData);
            var service = new UserService(context);

            // Act
            var result = await service.GetAllEmployees<EmployeeDataViewModel>();

            // Assert
            Assert.AreEqual(employeeData.Count(), result.Count());
            Assert.AreEqual(employeeData.Count(), context.EmployeeData.Count());
        }

        [Test]
        public async Task UpdateAsync_ShouldAddEmployeeData()
        {
            // Arange
            List<EmployeeData> employeeData = new() { Users.EmployeeUser1};

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(employeeData);
            var service = new UserService(context);

            // Act
            await service.UpdateAsync(Users.EmployeeUser2);

            // Assert
            Assert.AreEqual(employeeData.Count()+1, context.EmployeeData.Count());
        }
        /*
        public async Task<IEnumerable<T>> GetEmployeePageItems<T>(int page, int usersOnPage)
        {
            return await GetAllEmployees<T>().GetPageItems(page,usersOnPage);
        }

        public async Task<IEnumerable<T>> GetUserPageItems<T>(int page, int usersOnPage)
        {
            return await GetAllUsers<T>().GetPageItems(page, usersOnPage);
        }

        public async Task DeleteAsync(string id)
        {
            var userInContext = await context.EmployeeData.FindAsync(id);
            if (userInContext != null)
            {
                userInContext.DateOfResignation = DateTime.UtcNow;
                userInContext.IsActive = false;
                context.EmployeeData.Update(userInContext);
                await context.SaveChangesAsync();
            }
        }

      
        public async Task<ClientData> CreateClient(string email, string name, bool adult)
        {
            var client = new ClientData
            {
                Email = email,
                FullName = name,
                IsAdult = adult,
            };

            context.ClientData.Add(client);
            await context.SaveChangesAsync();

            return client;
        }

        public async Task<ClientData> UpdateClient(string id, string email, string name, bool adult)
        {
            var client = new ClientData
            {
                Id=id,
                Email = email,
                FullName = name,
                IsAdult = adult,
            };

            context.Update(client);
            await context.SaveChangesAsync();

            return client;
        }

        public async Task DeleteClient(string id)
        {
            var client = await context.ClientData.FindAsync(id);
            if(client!=null)
            {
                context.ClientData.Remove(client);
                await context.SaveChangesAsync();
            }
        }
         */

    }
}
