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

        [OneTimeSetUp]
        public void Setup()
        {
            MappingConfig.RegisterMappings(typeof(ErrorViewModel).Assembly);
        }

        [Test]
        public async Task GetEmployeeAsync_ShouldFindEmployee()
        {
            // Arange
            List<EmployeeData> employeeData = new() { Users.EmployeeUser1 };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(employeeData);
            var service = new UserService(context);

            // Act
            var result = await service.GetEmployeeAsync<EmployeeDataViewModel>(employeeData.First().UserId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(employeeData.First().UserId, result.UserId);
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
            Assert.AreEqual(userData.First().Id, result.Id);
        }

        [Test]
        public async Task AddAsync_ShouldAddEmployee()
        {
            // Arange
            List<ApplicationUser> userData = new() { Users.User1Employee, Users.User2Employee };
            List<EmployeeData> employeeData = new() { Users.EmployeeUser1 };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(userData)
                                                                .SeedAsync(employeeData);
            var service = new UserService(context);

            // Act
            await service.AddAsync(Users.EmployeeUser2);


            // Assert
            Assert.AreEqual(employeeData.Count + 1, context.Users.Count());
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

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(userData);
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
            List<EmployeeData> employeeData = new() { Users.EmployeeUser1 };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(employeeData);
            var service = new UserService(context);

            // Act
            await service.UpdateAsync(Users.EmployeeUser2);

            // Assert
            Assert.AreEqual(employeeData.Count() + 1, context.EmployeeData.Count());
        }

        [Test]
        public async Task GetEmployeePageItems_ShouldReturnAllEmployeesOnPage()
        {
            // Arange
            List<EmployeeData> employeesData = new() { Users.EmployeeUser1, Users.EmployeeUser2 };


            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(employeesData);
            var service = new UserService(context);

            // Act
            var employees = await service.GetEmployeePageItems<EmployeeDataViewModel>(1, 2);

            // Assert
            Assert.NotNull(employees);
            Assert.AreEqual(employeesData.Count, employees.Count());
        }

        [Test]
        public async Task GetUserPageItems_ShouldReturnAllUsersOnPage()
        {
            // Arange
            List<ApplicationUser> usersData = new() { Users.User1Employee, Users.User2Employee };


            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(usersData);
            var service = new UserService(context);

            // Act
            var users = await service.GetUserPageItems<UserDataViewModel>(1, 2);

            // Assert
            Assert.NotNull(users);
            Assert.AreEqual(usersData.Count, users.Count());
        }

        [Test]
        public async Task DeleteAsync_ShouldDeleteUsers()
        {
            // Arange
            List<EmployeeData> employeesData = new() { Users.EmployeeUser1, Users.EmployeeUser2 };


            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(employeesData);
            var service = new UserService(context);
            // Act
            await service.DeleteAsync(employeesData.First().UserId);

            // Assert
            Assert.AreNotEqual(employeesData.First().DateOfResignation, null);
        }

        [Test]
        public async Task DeleteClient_ShouldRemoveClient()
        {
            // Arange
            List<ClientData> clientsData = new() { Users.Client1User, Users.Client2User };
            List<ApplicationUser> usersData = new() { Users.User1Employee, Users.User2Employee };


            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(clientsData)
                                                                .SeedAsync(usersData);
            var service = new UserService(context);
            // Act
            await service.DeleteClient(clientsData.First().Id);

            // Assert
            Assert.AreEqual(context.ClientData.Count(), 1);
        }

        [Test]
        public async Task CreateClient_ShouldCreateClient()
        {
            // Arange
            List<ClientData> clientsData = new() { Users.Client1User, Users.Client2User };


            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(clientsData);
            var service = new UserService(context);
            // Act
            await service.CreateClient(Users.searchParam, Users.searchParam, true);

            // Assert
            Assert.AreEqual(context.ClientData.Count(), 3);
        }

        [Test]
        public async Task UpdateClient_ShouldUpdateClient()
        {
            // Arange
            List<ClientData> clientsData = new() { Users.ClientForSearch};


            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(clientsData);
            await context.SaveChangesAsync();

            var service = new UserService(context);
            // Act
            var updatedClient = await service.UpdateClient(Users.ClientForSearch.Id, Users.searchParam2, Users.searchParam2, true);

            // Assert
            Assert.AreEqual(Users.searchParam2, updatedClient.Email);
        }
    }
}
