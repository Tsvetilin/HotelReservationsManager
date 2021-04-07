using Data;
using Data.Models;
using System.Threading.Tasks;

namespace Services.Data
{
    public class SettingService : ISettingService
    {
        private readonly ApplicationDbContext dbContext;

        public SettingService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddAsync(string key, string value, string type)
        {
            await dbContext.Settings.AddAsync(
                new Setting
                {
                    Key = key,
                    Value = value,
                    Type = type,
                });

            await dbContext.SaveChangesAsync();
        }

        public async Task<(string Value, string Type)> GetAsync(string key)
        {
            var res = await dbContext.Settings.FindAsync(key);
            
            return res == null ? (null, null) : (res.Value, res.Type);
        }

        public async Task UpdateAsync(string key, string value, string type)
        {
            var res = await dbContext.Settings.FindAsync(key);
            
            if (res != null)
            {
                res.Value = value;
                res.Type = type;
                this.dbContext.Settings.Update(res);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
