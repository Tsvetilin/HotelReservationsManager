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

        public Task<T> GetAsync<T>(string id);

        public Task<IEnumerable<T>> GetAll<T>();

        //public Task<IEnumerable<T>> GetAllByUserName<T>(string username);

        //public Task<IEnumerable<T>> GetAllByFirstName<T>(string firstName);

        //public Task<IEnumerable<T>> GetAllBySecondName<T>(string secondName);

        public Task<IEnumerable<string>> GetAllByFamilyName(string familyName);

        public Task<IEnumerable<string>> GetAllByEmail(string email);

        public Task<IEnumerable<T>> GetPageItems<T>(int page, int usersOnPage);

        public Task<IEnumerable<T>> GetSearchResults<T>(string searchString);

        public Task UpdateAsync(EmployeeData user);

        public Task DeleteAsync(string id);
        public int CountAllEmployees();

        public bool IsAlreadyAdded(string email);
    }
}
