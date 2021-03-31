using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Services.Mapping;
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

        public async Task<IEnumerable<T>> GetAllByUserName<T>(string username)
        {
            return await context.Users.Where(x => x.UserName == username).ProjectTo<T>().ToListAsync();
        }
        public async Task<IEnumerable<T>> GetAllByFirstName<T>(string firstName)
        {
            return await context.Users.Where(x => x.FirstName == firstName).ProjectTo<T>().ToListAsync();
        }
        public async Task<IEnumerable<T>> GetAllBySecondName<T>(string secondName)
        {
            return await context.EmployeeData.Where(x => x.SecondName == secondName).ProjectTo<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllByFamilyName<T>(string familyName)
        {
            return await context.EmployeeData.Where(x => x.SecondName == familyName).ProjectTo<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllByEmail<T>(string email)
        {
            return await context.Users.Where(x => x.Email == email).ProjectTo<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetPageItems<T>(int page, int usersOnPage)
        {
            var users = await GetAll<T>();
            return users.Skip(usersOnPage * (page - 1)).Take(usersOnPage).AsQueryable().ProjectTo<T>().ToList();
        }

        public async Task<IEnumerable<T>> GetSearchResults<T>(string searchString)
        {
            var result = new List<T>();

            var emailResults = await GetAllByEmail<T>(searchString);
            var firstNameResults = await GetAllByFirstName<T>(searchString);
            var secondNameResults = await GetAllBySecondName<T>(searchString);
            var familyNameResults = await GetAllByFamilyName<T>(searchString);
            var userNameResults = await GetAllByUserName<T>(searchString);


            if (emailResults != null)
            {
                result.AddRange(emailResults);
            }

            if (firstNameResults != null)
            {
                result.AddRange(firstNameResults);
            }

            if (secondNameResults != null)
            {
                result.AddRange(secondNameResults);
            }

            if (familyNameResults != null)
            {
                result.AddRange(familyNameResults);
            }

            if (userNameResults != null)
            {
                result.AddRange(userNameResults);
            }

            return result;
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
                context.EmployeeData.Remove(userInContext);
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
