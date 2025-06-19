using Microsoft.Extensions.FileProviders;
using tutorialRazorPages.DataBase;
using WebApp_Feed;
using WebApp_Feed.Database;
using WebApp_Landing;
using WebApp_Landing.Services;

namespace WebApp_EFDB
{
    public static class MiddlewareStaticExtension
    {
        public static IApplicationBuilder UseLocalStaticFiles(this IApplicationBuilder app, string projectFolder)
        {
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), projectFolder, "wwwroot"))
            });
            return app;
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddHttpClient(); 
            builder.Services.AddScoped<ITelegramService, TelegramService>();


            // Add Feed Controllers
            builder.Services.AddFeedControllers();
            builder.Services.AddFeedDatabase(builder.Configuration.GetConnectionString("SQLiteConnection"));

            // Add Landing Pages
            builder.Services.AddLandingPages();



            var app = builder.Build();            

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<GreenswampContext>();
                DbInitializer.Initialize(dbContext);
            }

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler();
            }

            // Add static files from feed
            app.UseLocalStaticFiles("WebApp-Feed");

            // Add static files from landing
            app.UseLocalStaticFiles("WebApp-Landing");

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapAreaControllerRoute(
                    name: "feed_area",
                    areaName: "Feed",
                    pattern: "/Feed/{controller=Home}/{action=Index}/{id?}");

            });

            app.Run();
        }
    }
}