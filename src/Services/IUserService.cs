using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IUserService
    {
        public Task AddAsync(EmployeeData user);

        public Task<EmployeeData> GetAsync(string id);

        public IEnumerable<EmployeeData> GetAll();

        public  Task<IEnumerable<T>> GetAllByUserName<T>(string username) where T : class;

        public Task<IEnumerable<T>> GetAllByFirstName<T>(string firstName) where T : class;

        public Task<IEnumerable<T>> GetAllBySecondName<T>(string secondName) where T : class;

        public Task<IEnumerable<T>> GetAllByFamilyName<T>(string familyName) where T : class;

        public Task<IEnumerable<T>> GetAllByEmail<T>(string email) where T : class;

        public Task UpdateAsync(EmployeeData user);

        public Task DeleteAsync(string id);
    }
}
