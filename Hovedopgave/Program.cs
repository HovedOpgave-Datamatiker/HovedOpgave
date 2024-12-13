using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Hovedopgave.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
namespace Hovedopgave
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<HovedopgaveContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("HovedopgaveContext") ?? throw new InvalidOperationException("Connection string 'HovedopgaveContext' not found.")));

            builder.Services.AddControllersWithViews();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();


            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
