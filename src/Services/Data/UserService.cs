using Data;
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
namespace Services.Data
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext context;

        public UserService(ApplicationDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Adds employee to the database
        /// </summary>
        /// <param name="user">New employee object</param>
        /// <returns>Task representing the operation</returns>
        public async Task AddAsync(EmployeeData user)
        {
            await context.EmployeeData.AddAsync(user);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Finds employee with the searched id
        /// </summary>
        /// <typeparam name="T">Data class to map employee data to</typeparam>
        /// <param name="id">Employee id to search for</param>
        /// <returns>Task with the employee data parsed to <typeparamref name="T"/>
        /// object or null if not found</returns>
        public async Task<T> GetEmployeeAsync<T>(string id)
        {
            return await context.EmployeeData.Where(x => x.UserId == id).ProjectTo<T>().FirstOrDefaultAsync();
        }

        /// <summary>
        /// Finds user with the searched id
        /// </summary>
        /// <typeparam name="T">Data class to map user data to</typeparam>
        /// <param name="id">User id to search for</param>
        /// <returns>Task with the user data parsed to <typeparamref name="T"/>
        /// object or null if not found</returns>
        public async Task<T> GetUserAsync<T>(string id)
        {
            return await context.Users.Where(x => x.Id == id).ProjectTo<T>().FirstOrDefaultAsync();
        }

        /// <summary>
        /// Finds all employees
        /// </summary>
        /// <typeparam name="T">Data class to map employees data to</typeparam>
        /// <returns>Task with the employees data parsed to <typeparamref name="T"/>
        /// object or null if not found</returns>
        public async Task<IEnumerable<T>> GetAllEmployees<T>()
        {
            return await context.EmployeeData.AsQueryable().ProjectTo<T>().ToListAsync();
        }

        /// <summary>
        /// Finds all users
        /// </summary>
        /// <typeparam name="T">Data class to map users data to</typeparam>
        /// <returns>Task with the users data parsed to <typeparamref name="T"/>
        /// object or null if not found</returns>
        public async Task<IEnumerable<T>> GetAllUsers<T>()
        {
            return await context.Users.AsQueryable().ProjectTo<T>().ToListAsync();
        }

        /// <summary>
        /// Finds all users with first name, last name,
        /// username or email containing the <paramref name="searchString"/>
        /// </summary>
        /// <param name="searchString">User first name, last name, username or email to search for</param>
        /// <returns>Task with the ids of the found employees parsed to string object or 
        /// null if not found
        public async Task<IEnumerable<string>> GetAllBySearch(string searchString)
        {
            return await context.Users.Where(x => x.Email.ToUpper().Contains(searchString.ToUpper()) ||
                                             x.FirstName.ToUpper().Contains(searchString.ToUpper()) ||
                                             x.LastName.ToUpper().Contains(searchString.ToUpper()) ||
                                             x.UserName.ToUpper().Contains(searchString.ToUpper())).
                                             Select(x => x.Id).ToListAsync();
        }

        /// <summary>
        /// Finds all employees by a selected second name
        /// </summary>
        /// <param name="searchString">Employee second name to search for</param>
        /// <returns>Task with the ids of the found employees parsed to string object or 
        /// null if not found</returns>
        public async Task<IEnumerable<string>> GetAllBySecondName(string searchString)
        {
            return await context.EmployeeData.Where(x => x.SecondName.ToUpper().Contains(searchString.ToUpper())).
                                                    Select(x => x.UserId).ToListAsync();
        }

        /// <summary>
        /// Finds employees according to specified pagination rules
        /// </summary>
        /// <typeparam name="T">Data class to map employees data to</typeparam>
        /// <param name="page">The number of current page</param>
        /// <param name="usersOnPage">The number of users on the page</param>
        /// <returns>Task with employee data parsed to <typeparamref name="T"/>
        /// object or null if not found</returns>
        public async Task<IEnumerable<T>> GetEmployeePageItems<T>(int page, int usersOnPage)
        {
            return await GetAllEmployees<T>().GetPageItems(page, usersOnPage);
        }

        /// <summary>
        /// Finds users according to specified pagination rules
        /// </summary>
        /// <typeparam name="T">Data class to map users data to</typeparam>
        /// <param name="page">The number of current page</param>
        /// <param name="usersOnPage">The number of users on the page</param>
        /// <returns>Task with users data parsed to <typeparamref name="T"/>
        /// object or null if not found</returns>
        public async Task<IEnumerable<T>> GetUserPageItems<T>(int page, int usersOnPage)
        {
            return await GetAllUsers<T>().GetPageItems(page, usersOnPage);
        }

        /// <summary>
        /// Finds all users that satisfy the criteria
        /// </summary>
        /// <param name="searchString">User first name, second name, last name, username or email to search for</param>
        /// <returns>Task with users ids that satisfy the criteria</returns>
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

        /// <summary>
        /// Finds all employees that satisfy the criteria
        /// </summary>
        /// <typeparam name="T">Data class to map employees data to</typeparam>
        /// <param name="searchString">User first name, second name, last name, username or email to search for</param>
        /// <returns>Task with employees data that satisfy the criteria parsed to <typeparamref name="T"/>
        /// object or null if not found</returns>
        public async Task<IEnumerable<T>> GetEmployeesSearchResults<T>(string searchString)
        {
            List<string> result = await GetSearchResults(searchString);

            return await context.EmployeeData.Where(x => result.Contains(x.UserId)).ProjectTo<T>().ToListAsync();
        }

        /// <summary>
        /// Finds all users that satisfy the criteria
        /// </summary>
        /// <typeparam name="T">Data class to map employees data to</typeparam>
        /// <param name="searchString">User first name, second name, last name, username or email to search for</param>
        /// <returns>Task with users data that satisfy the criteria parsed to <typeparamref name="T"/>
        /// object or null if not found</returns>
        public async Task<IEnumerable<T>> GetUsersSearchResults<T>(string searchString)
        {
            List<string> result = await GetSearchResults(searchString);

            return await context.Users.Where(x => result.Contains(x.Id)).ProjectTo<T>().ToListAsync();
        }

        /// <summary>
        /// Updates existing employee's data in database or hires a user as employee
        /// </summary>
        /// <param name="user">Existing employee object</param>
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

        /// <summary>
        /// Hires employee and makes him inactive
        /// </summary>
        /// <param name="id">Existing employee id</param>
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

        /// <returns>
        /// The count of all employees in the database
        /// </returns>
        public int CountAllEmployees()
        {
            return context.EmployeeData.Count();
        }


        /// <returns>
        /// The count of all users in the database
        /// </returns>
        public int CountAllUsers()
        {
            return context.Users.Count();
        }

        /// <summary>
        /// Checks if user is already added to the database
        /// </summary>
        /// <param name="email">The email of the searched user</param>
        /// <returns>True, if the name of the user exists, or false if not</returns>
        public bool IsAlreadyAdded(string email)
        {
            return context.Users.Any(x => x.Email.ToLower().Equals(email.ToLower()));
        }

        /// <summary>
        /// Adds new client to database
        /// </summary>
        /// <param name="email">New client's email</param>
        /// <param name="name">New client's full name</param>
        /// <param name="adult">If the new client is adult or not</param>
        /// <returns>Task with the new client data</returns>
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

        /// <summary>
        /// Udates client's data
        /// </summary>
        /// <param name="id">Existing client's id</param>
        /// <param name="email">Existing client's email</param>
        /// <param name="name">Existing client's full name</param>
        /// <param name="adult">If the Existing client is adult or not</param>
        /// <returns>Task with the updated client data</returns>
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

        /// <summary>
        /// Removes client from database
        /// </summary>
        /// <param name="id">The searched client's id</param>
        /// <returns>Task representing the operation</returns>
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
