using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blogger.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Blogger
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();  // Create app

            // Get the middleware services from the Startup class through dependency injection
            var scope = host.Services.CreateScope();

            try
            {
                var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                // Seed Database
                ctx.Database.EnsureCreated();       // Make sure DB is created

                var adminRole = new IdentityRole("Admin");
                if (!ctx.Roles.Any())
                {
                    // Create role
                    roleMgr.CreateAsync(adminRole).GetAwaiter().GetResult();
                }

                if (!ctx.Users.Any(u => u.UserName == "Admin"))
                {
                    // Create an admin
                    var adminUser = new IdentityUser
                    {
                        UserName = "Admin",
                        Email = "test@test.com"
                    };

                    userMgr.CreateAsync(adminUser, "password").GetAwaiter().GetResult();

                    // Add role to user
                    userMgr.AddToRoleAsync(adminUser, adminRole.Name).GetAwaiter().GetResult();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            host.Run();     // Run app
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
