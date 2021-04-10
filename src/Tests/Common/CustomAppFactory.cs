using Data;
using Data.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Linq;
using System.Security.Claims;
using Web;

/// <summary>
/// Tests project space related to commonly used helpers & factories
/// </summary>
namespace Tests.Common
{
    /// <summary>
    /// Custom application factory that bypasses the Authentication and Authorization 
    /// and creates dummy SQLServer database for the tests that is later automatically deleted
    /// </summary>
    public class CustomAppFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
            builder.ConfigureServices(services =>
            {
                // Database change
                var db = services.SingleOrDefault(
                     d => d.ServiceType ==
                typeof(ApplicationDbContext));

                var dbo = services.SingleOrDefault(
                     d => d.ServiceType ==
                typeof(DbContextOptions<ApplicationDbContext>));

                services.Remove(db);
                services.Remove(dbo);

                services.AddDbContext<ApplicationDbContext>((options, context) =>
                {
                    //Safe Connection string, but memory intense ->TestDatabaseConnectionProvider.GetConnectionStringDisposable()
                    //Unsafe Connection string -> TestDatabaseConnectionProvider.Instance.SharedConnectionStringDisposable
                    context.UseSqlServer(TestDatabaseConnectionProvider.Instance.SharedConnectionStringDisposable);
                });

                // Authentication bypass
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
