using System.Threading.Tasks;

namespace Services.Data
{
    public interface ISettingService
    {
        public Task AddAsync(string key, string value, string type);
        public Task<(string Value, string Type)> GetAsync(string key);
        public Task UpdateAsync(string key, string value, string type);
    }
}