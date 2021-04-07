using Data;
using Data.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Services;
using Services.Data;
using System.Linq;

namespace Tests.Common
{
    public class CustomWebAppFactory <TStartup>: WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                //services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("TestDb"));
                //services.AddSingleton(x => InMemoryFactory.InitializeContext());
                //services.AddSingleton<ISettingService, SettingService>();
                //services.AddSingleton<IUserService, IUserService>();
                //services.AddSingleton(x => new UserManager<ApplicationUser>(null, null, null, null, null, null, null, null,null)) ;
                //services.AddSingleton<IMemoryCache, MemoryCache>();
                ///*
                // IReservationService reservationService,
                //                      IUserService userService,
                //                      IRoomService roomService,
                //                      ISettingService settingService,
                //                      UserManager<ApplicationUser> userManager,
                //                      IMemoryCache memoryCache*/
                //services.AddSingleton<AuthenticationHandler<AuthenticationSchemeOptions>, TestAuthHandler>();
            });
        }
    }
}
