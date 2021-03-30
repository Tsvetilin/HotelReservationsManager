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
            return mapper.Map<List<ApplicationUser>, IEnumerable<T>>(employeesInContext);
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
    }
}
