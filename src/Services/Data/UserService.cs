using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Services.Common;
using Services.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<T> GetAsync<T>(string id)
        {
            return await context.EmployeeData.Where(x => x.UserId == id).ProjectTo<T>().FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetAll<T>()
        {
            return await context.EmployeeData.AsQueryable().ProjectTo<T>().ToListAsync();
        }

        //public async Task<IEnumerable<T>> GetAllByUserName<T>(string username)
        //{
        //    return await context.Users.Where(x => x.UserName == username).ProjectTo<T>().ToListAsync();
        //}
        //public async Task<IEnumerable<T>> GetAllByFirstName<T>(string firstName)
        //{
        //    return await context.Users.Where(x => x.FirstName == firstName).ProjectTo<T>().ToListAsync();
        //}
        //public async Task<IEnumerable<T>> GetAllBySecondName<T>(string secondName)
        //{
        //    return await context.EmployeeData.Where(x => x.SecondName == secondName).ProjectTo<T>().ToListAsync();
        //}

        public async Task<IEnumerable<string>> GetAllByFamilyName(string searchString)
        {
            return await context.Users.Where(x => x.Email == searchString ||
                                             x.FirstName == searchString ||
                                             x.LastName == searchString ||
                                             x.UserName == searchString).Select(x => x.Id).ToListAsync();
        }

        public async Task<IEnumerable<string>> GetAllByEmail(string searchString)
        {
            return await context.EmployeeData.Where(x => x.SecondName == searchString).Select(x=>x.UserId).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetPageItems<T>(int page, int usersOnPage)
        {
            return await GetAll<T>().GetPageItems(page,usersOnPage);
        }

       //Dublirane
        public async Task<IEnumerable<T>> GetSearchResults<T>(string searchString)
        {
            var result = new List<string>();

            var emailResults = await GetAllByEmail(searchString);
            //var firstNameResults = await GetAllByFirstName<T>(searchString);
            //var secondNameResults = await GetAllBySecondName<T>(searchString);
            var familyNameResults = await GetAllByFamilyName(searchString);
            //var userNameResults = await GetAllByUserName<T>(searchString);


            if (emailResults != null)
            {
                result.AddRange(emailResults);
            }

            //if (firstNameResults != null)
            //{
            //    result.AddRange(firstNameResults);
            //}

            //if (secondNameResults != null)
            //{
            //    result.AddRange(secondNameResults);
            //}

            if (familyNameResults != null)
            {
                result.AddRange(familyNameResults);
            }

            //if (userNameResults != null)
            //{
            //    result.AddRange(userNameResults);
            //}
            result = result.Distinct().ToList();

            return await context.EmployeeData.Where(x=>result.Contains(x.UserId)).ProjectTo<T>().ToListAsync();
        }

        public async Task UpdateAsync(EmployeeData user)
        {
            var userInContext = await context.EmployeeData.FindAsync(user.UserId);
            if (userInContext != null)
            {
                context.Entry(userInContext).CurrentValues.SetValues(user);
                await context.SaveChangesAsync();
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

        public bool IsAlreadyAdded(string email)
        {
            return context.Users.Any(x => x.Email.ToLower().Equals(email.ToLower()));
        }
    }
}
