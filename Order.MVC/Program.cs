using Order.MVC.Data;
using System;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Order.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            var version = new MySqlServerVersion(new Version(10, 6, 19));

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<OrderContext>(options =>
                options.UseMySql(connectionString, version));

            // Configurar a globalização para o formato brasileiro
            var supportedCultures = new[] { new CultureInfo("pt-BR") };

            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                // Definir a cultura padrão como pt-BR
                var ptBR = new CultureInfo("pt-BR");

                options.DefaultRequestCulture = new RequestCulture(ptBR);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            var app = builder.Build();
            
            // Create and migrate database on startup, regardless of user access
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<OrderContext>();
                    
                    // This will create the database if it doesn't exist
                    context.Database.EnsureCreated();
                    
                    // Apply any pending migrations
                    if (context.Database.GetPendingMigrations().Any())
                    {
                        context.Database.Migrate();
                    }
                    
                    Console.WriteLine("Database initialization completed successfully.");
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while initializing the database.");
                    
                    // Write to console for immediate visibility during deployment
                    Console.WriteLine($"Database initialization error: {ex.Message}");
                    Console.WriteLine(ex.StackTrace);
                }
            }
            
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("pt-BR"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Aplicar as configurações de localização antes dos outros middlewares
            app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
