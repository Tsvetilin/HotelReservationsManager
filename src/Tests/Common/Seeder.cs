using Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tests.Common
{
    /// <summary>
    /// Database seeder extensions
    /// </summary>
    public static class Seeder
    {
        public static async Task<ApplicationDbContext> SeedAsync<T>(this ApplicationDbContext context, IEnumerable<T> data)
        {
            foreach (var item in data)
            {
                await context.AddAsync(item);
            }
            await context.SaveChangesAsync();
            return context;
        }

        public static async Task<ApplicationDbContext> SeedAsync<T>(this Task<ApplicationDbContext> contextTask, IEnumerable<T> data)
        {
            var context = await contextTask;

            foreach (var item in data)
            {
                await context.AddAsync(item);
            }

            await context.SaveChangesAsync();
            return context;
        }
    }
}
