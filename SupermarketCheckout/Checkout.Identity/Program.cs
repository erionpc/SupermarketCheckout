using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Checkout.Identity.Data.Contexts;
using Microsoft.AspNetCore.Identity;
using Checkout.Identity.Models;
using Checkout.Identity.Services;
using Checkout.Identity.Data.Migrations;

namespace Checkout.Identity
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            // migrate the database.  Best practice = in Main, using service scope
            using (var scope = host.Services.CreateScope())
            {
                runDbMigrations(scope);
                await initialiseAuthDbAsync(scope);
            }

            // run the web app
            host.Run();
        }

        private static void runDbMigrations(IServiceScope scope)
        {
            try
            {
                var authDbContext = scope.ServiceProvider.GetService<AuthDbContext>();
                authDbContext.Database.Migrate();
            }
            catch (Exception ex)
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while migrating the databases.");
            }
        }

        private static async Task initialiseAuthDbAsync(IServiceScope scope)
        {
            try
            {
                var authManager = scope.ServiceProvider.GetService<IAuthService>();
                await authManager.InitialiseAuthData();
            }
            catch (Exception ex)
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while initialising auth data.");
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
