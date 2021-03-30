using AutoMapper;
using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class UserService: IUserService
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public UserService(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task AddAsync(EmployeeData user)
        {
            await context.EmployeeData.AddAsync(user);
            await context.SaveChangesAsync();
        }

        public async Task<EmployeeData> GetAsync(string id)
        {
            return await context.EmployeeData.FindAsync(id);
        }

        public IEnumerable<EmployeeData> GetAll()
        {
            return context.EmployeeData.ToList();
        }

        public async Task<IEnumerable<T>> GetAllByUserName<T>(string username) where T : class
        {
            var employeesInContext = await context.Users.Where(x => x.UserName == username).ToListAsync();
            return mapper.Map<List<ApplicationUser>, IEnumerable<T>>(employeesInContext);
        }
        public async Task<IEnumerable<T>> GetAllByFirstName<T>(string firstName) where T : class
        {
            var employeesInContext = await context.Users.Where(x => x.FirstName == firstName).ToListAsync();
            return  mapper.Map<List<ApplicationUser>, IEnumerable<T>>(employeesInContext);
        }
        public async Task<IEnumerable<T>> GetAllBySecondName<T>(string secondName) where T : class
        {
            var employeesInContext = await context.EmployeeData.Where(x => x.SecondName == secondName).ToListAsync();
            return mapper.Map<List<EmployeeData>, IEnumerable<T>>(employeesInContext);
        }

        public async Task<IEnumerable<T>> GetAllByFamilyName<T>(string familyName) where T : class
        {
            var employeesInContext = await context.EmployeeData.Where(x => x.SecondName == familyName).ToListAsync();
            return mapper.Map<List<EmployeeData>, IEnumerable<T>>(employeesInContext);
        }

        public async Task<IEnumerable<T>> GetAllByEmail<T>(string email) where T : class
        {
            var employeesInContext = await context.Users.Where(x => x.Email == email).ToListAsync();
            return mapper.Map<List<ApplicationUser>, IEnumerable<T>>(employeesInContext);
        }

        public IEnumerable<T> GetPageItems<T>(int page, int usersOnPage)
        {
            var users = GetAll();
            var selectedUsers = users.Skip(usersOnPage * (page - 1)).Take(usersOnPage).ToList();
            return mapper.Map<List<EmployeeData>, IEnumerable<T>>(selectedUsers); 
        }

        public IEnumerable<T> GetSearchResults<T>(string searchString)
        {
            var result = new List<T>();
            var emailResults = mapper.Map<IEnumerable<T>, List<T>>((IEnumerable<T>)GetAllByEmail<EmployeeData>(searchString));
            if (emailResults!=null)
            {
                result.AddRange(emailResults);
            }

            var firstNameResults = mapper.Map<IEnumerable<T>, List<T>>((IEnumerable<T>)GetAllByFirstName<EmployeeData>(searchString));
            if (firstNameResults != null)
            {
                result.AddRange(firstNameResults);
            }

            var secondNameResults = mapper.Map<IEnumerable<T>, List<T>>((IEnumerable<T>)GetAllBySecondName<EmployeeData>(searchString));
            if (secondNameResults != null)
            {
                result.AddRange(secondNameResults);
            }

            var familyNameResults = mapper.Map<IEnumerable<T>, List<T>>((IEnumerable<T>)GetAllByFamilyName<EmployeeData>(searchString));
            if (familyNameResults != null)
            {
                result.AddRange(familyNameResults);
            }

            var userNameResults = mapper.Map<IEnumerable<T>, List<T>>((IEnumerable<T>)GetAllByUserName<EmployeeData>(searchString));
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
