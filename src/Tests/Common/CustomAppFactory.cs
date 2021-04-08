using Data;
using Data.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Linq;
using System.Security.Claims;
using Web;
using static Tests.Common.TestAuthHandler;

namespace Tests.Common
{
    public class CustomAppFactory : WebApplicationFactory<Startup>
    {

        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            var builder = WebHost.CreateDefaultBuilder(Array.Empty<string>());
            builder.UseStartup<Startup>();
            builder.UseSetting("Environment", "Test");
            return builder;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
            builder.ConfigureServices(services =>
            {
                var s = services.SingleOrDefault(
                     d => d.ServiceType ==
                typeof(ApplicationDbContext));

                services.Remove(s);
                services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(TestDatabaseConnection.TestingConnectionString));
                var a = services.SingleOrDefault(
                     d => d.ServiceType ==
                typeof(AuthenticationBuilder));

                services.Remove(a);
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "Test";
                    options.DefaultChallengeScheme = "Test";
                }).AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });

                var um = services.SingleOrDefault(
                     d => d.ServiceType ==
                typeof(UserManager<ApplicationUser>));

                services.Remove(um);

                var store = new Mock<IUserStore<ApplicationUser>>();
                var mgr = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
                mgr.Object.UserValidators.Add(new UserValidator<ApplicationUser>());
                mgr.Object.PasswordValidators.Add(new PasswordValidator<ApplicationUser>());

                var name = "Admin";
                var email = "admin@hms.com";
                var admin = new ApplicationUser
                {
                    Id = name,
                    Email = email,
                    NormalizedEmail = email.ToUpper(),
                    UserName = name,
                    NormalizedUserName = name.ToUpper(),
                    EmailConfirmed = true,
                    IsAdult = true,
                    FirstName = name,
                    LastName = name,
                };

                mgr.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(admin);
                services.AddTransient(d => mgr.Object);

                services.AddControllersWithViews(options =>
                {
                    options.Filters.Add(typeof(FakeUserFilter));
                });

            });
        }
    }
}
