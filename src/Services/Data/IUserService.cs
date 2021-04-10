using Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Data
{
    public interface IUserService
    {
        public Task AddAsync(EmployeeData user);
        public Task<T> GetEmployeeAsync<T>(string id);
        public Task<T> GetUserAsync<T>(string id);
        public Task<IEnumerable<T>> GetAllEmployees<T>();
        public Task<IEnumerable<T>> GetAllUsers<T>();
        public Task<IEnumerable<string>> GetAllBySearch(string familyName);
        public Task<IEnumerable<string>> GetAllBySecondName(string email);
        public Task<IEnumerable<T>> GetEmployeePageItems<T>(int page, int usersOnPage);
        public Task<IEnumerable<T>> GetUserPageItems<T>(int page, int usersOnPage);
        public Task<IEnumerable<T>> GetEmployeesSearchResults<T>(string searchString);
        public Task<IEnumerable<T>> GetUsersSearchResults<T>(string searchString);
        public Task UpdateAsync(EmployeeData user);
        public Task DeleteAsync(string id);
        public int CountAllEmployees();
        public int CountAllUsers();
        public bool IsAlreadyAdded(string email);
        public Task<ClientData> CreateClient(string email, string name, bool adult);
        public Task<ClientData> UpdateClient(string id, string email, string name, bool adult);
        public Task DeleteClient(string id);
    }
}