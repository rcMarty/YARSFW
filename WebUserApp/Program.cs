using Microsoft.Extensions.Configuration;
using DataLayer;
using DataLayer.DatabaseEntites;


namespace WebUserApp
{
    public class Program
    {
        
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            DBConfig.ConnectionString = "C:\\Users\\Marty\\OneDrive - VSB-TUO\\Dokumenty\\_Škola\\4_Semestr\\C#\\projekt\\DataLayer\\assets\\Wellness.db"; // use your own path to database
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }

    }
}