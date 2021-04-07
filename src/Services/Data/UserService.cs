﻿using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Services.Common;
using Services.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Service layer logic
/// </summary>
namespace Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext context;

        public UserService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(EmployeeData user)
        {
            await context.EmployeeData.AddAsync(user);
            await context.SaveChangesAsync();
        }

        public async Task<T> GetEmployeeAsync<T>(string id)
        {
            return await context.EmployeeData.Where(x => x.UserId == id).ProjectTo<T>().FirstOrDefaultAsync();
        }

        public async Task<T> GetUserAsync<T>(string id)
        {
            return await context.Users.Where(x => x.Id == id).ProjectTo<T>().FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetAllEmployees<T>()
        {
            return await context.EmployeeData.AsQueryable().ProjectTo<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllUsers<T>()
        {
            return await context.Users.AsQueryable().ProjectTo<T>().ToListAsync();
        }

        public async Task<IEnumerable<string>> GetAllBySearch(string searchString)
        {
            return await context.Users.Where(x => x.Email.ToUpper().Contains(searchString.ToUpper()) ||
                                             x.FirstName.ToUpper().Contains(searchString.ToUpper()) ||
                                             x.LastName.ToUpper().Contains(searchString.ToUpper()) ||
                                             x.UserName.ToUpper().Contains(searchString.ToUpper())).
                                             Select(x => x.Id).ToListAsync();
        }

        public async Task<IEnumerable<string>> GetAllBySecondName(string searchString)
        {
            return await context.EmployeeData.Where(x => x.SecondName.ToUpper().Contains(searchString.ToUpper())).
                                                    Select(x => x.UserId).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetEmployeePageItems<T>(int page, int usersOnPage)
        {
            return await GetAllEmployees<T>().GetPageItems(page, usersOnPage);
        }

        public async Task<IEnumerable<T>> GetUserPageItems<T>(int page, int usersOnPage)
        {
            return await GetAllUsers<T>().GetPageItems(page, usersOnPage);
        }

        private async Task<List<string>> GetSearchResults(string searchString)
        {
            var result = new List<string>();

            var emailResults = await GetAllBySecondName(searchString);
            var familyNameResults = await GetAllBySearch(searchString);

            if (emailResults != null)
            {
                result.AddRange(emailResults);
            }

            if (familyNameResults != null)
            {
                result.AddRange(familyNameResults);
            }

            return result.Distinct().ToList();
        }

        public async Task<IEnumerable<T>> GetEmployeesSearchResults<T>(string searchString)
        {
            List<string> result = await GetSearchResults(searchString);

            return await context.EmployeeData.Where(x => result.Contains(x.UserId)).ProjectTo<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetUsersSearchResults<T>(string searchString)
        {
            List<string> result = await GetSearchResults(searchString);

            return await context.Users.Where(x => result.Contains(x.Id)).ProjectTo<T>().ToListAsync();
        }
        public async Task UpdateAsync(EmployeeData user)
        {
            var userInContext = await context.EmployeeData.FindAsync(user.UserId);
            
            if (userInContext != null)
            {
                context.Entry(userInContext).CurrentValues.SetValues(user);
                await context.SaveChangesAsync();
            }
            else
            {
                await AddAsync(user);
            }
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

        public int CountAllEmployees()
        {
            return context.EmployeeData.Count();
        }

        public int CountAllUsers()
        {
            return context.Users.Count();
        }

        public bool IsAlreadyAdded(string email)
        {
            return context.Users.Any(x => x.Email.ToLower().Equals(email.ToLower()));
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
                Id = id,
                Email = email,
                FullName = name,
                IsAdult = adult,
            };

            var clientInContext = await context.ClientData.FindAsync(id);
            context.Entry<ClientData>(clientInContext).CurrentValues.SetValues(client);
            await context.SaveChangesAsync();

            return client;
        }

        public async Task DeleteClient(string id)
        {
            var client = await context.ClientData.FindAsync(id);
            
            if (client != null)
            {
                context.ClientData.Remove(client);
                await context.SaveChangesAsync();
            }
        }
    }
}
