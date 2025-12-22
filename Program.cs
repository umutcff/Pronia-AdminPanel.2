using Microsoft.EntityFrameworkCore;
using ProniaUmut.Contexts;

namespace ProniaUmut
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<AppDBContext>(option=>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
            });

            var app = builder.Build();

            app.UseRouting();
            app.UseStaticFiles();

            app.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
                );
            
            app.MapDefaultControllerRoute();

            app.Run();
        }
    }
}
