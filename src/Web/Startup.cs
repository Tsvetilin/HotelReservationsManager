using Data;
using Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Services.Data;
using Services.External;
using Services.Mapping;

/// <summary>
/// Presentation layer entry point space
/// </summary>
namespace Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add database context and identity provider
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, ApplicationRole>(
                options =>
                {
                    options.Lockout.AllowedForNewUsers = true;
                    options.Lockout.MaxFailedAccessAttempts = 15;
                    options.User.RequireUniqueEmail = true;
                    //options.SignIn.RequireConfirmedAccount = true;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();

            // Add caching
            services.AddMemoryCache();

            // Add cookie policy
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Lax;
            });

            // Register views and pages
            services.AddControllersWithViews();
            services.AddRazorPages();

            // Register services
            services.AddTransient<IEmailSender>(x=>new EmailSender(Configuration.GetSection("SendGrid")["ApiKey"],
                                                                   Configuration.GetSection("SendGrid")["SenderEmail"], 
                                                                   Configuration.GetSection("SendGrid")["SenderName"]));

            services.AddTransient<IImageManager>(x => new ImageManager(Configuration.GetSection("Cloudinary")["CloudName"],
                                                                       Configuration.GetSection("Cloudinary")["ApiKey"],
                                                                       Configuration.GetSection("Cloudinary")["ApiSecret"]));

            services.AddTransient<ISettingService, SettingService>();  
            services.AddTransient<IUserService, UserService>();      
            services.AddTransient<IReservationService, ReservationsService>();
            services.AddTransient<IRoomService, RoomServices>();
        }

        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configure AutoMapper
            MappingConfig.RegisterMappings(this.GetType().Assembly);

            // Migrate and seed database if required
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
                new ApplicationDbContextSeeder()
                    .SeedAsync(dbContext,
                               serviceScope
                               .ServiceProvider
                               .GetService<ILoggerFactory>()
                               .CreateLogger(typeof(ApplicationDbContextSeeder)))
                    .GetAwaiter()
                    .GetResult();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            // Add authentication
            app.UseAuthentication();
            app.UseAuthorization();

            // Register routes
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                        name: "areaRoute",
                        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapRazorPages();
            });
        }
    }
}
