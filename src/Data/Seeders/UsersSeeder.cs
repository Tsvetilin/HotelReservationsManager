using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Seeders
{
    public class UsersSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, ILogger logger)
        {
            if (dbContext.Users.Any())
            {
                return;
            }

            var name = "Admin";
            var email = "admin@hsm.com";
            var user = new ApplicationUser
            {
                Email = email,
                NormalizedEmail = email.ToUpper(),
                UserName = name,
                NormalizedUserName = name.ToUpper(),
                EmailConfirmed = true,
                IsAdult = true,
                FirstName = name,
                LastName = name,
                LockoutEnabled = true,
                SecurityStamp = DateTime.UtcNow.Ticks.ToString(),
                ConcurrencyStamp = DateTime.UtcNow.Ticks.ToString()
            };

            var passwordHasher = new PasswordHasher<ApplicationUser>();
            user.PasswordHash = passwordHasher.HashPassword(user, "AdminPass");
            await dbContext.Users.AddAsync(user);


            await dbContext.SaveChangesAsync();
            logger.LogInformation($"Finished executing {nameof(UsersSeeder)}");

        }
    }
}
