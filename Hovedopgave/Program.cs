using Microsoft.EntityFrameworkCore;
using Hovedopgave.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Hovedopgave.Services;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config")]

namespace Hovedopgave
{
    public class Program
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<HovedopgaveContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("HovedopgaveContext")
                    ?? throw new InvalidOperationException("Connection string 'HovedopgaveContext' not found.")));

            builder.Services.AddControllersWithViews();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
            });

            // Read SMTP
            var smtpSettings = builder.Configuration.GetSection("SmtpSettings");
            var host = smtpSettings["Host"];
            var port = int.Parse(smtpSettings["Port"]);
            var enableSsl = bool.Parse(smtpSettings["EnableSsl"]);
            var username = smtpSettings["Username"];
            var password = smtpSettings["Password"];
            var fromAddress = smtpSettings["FromAddress"];

            builder.Services.AddSingleton(new NotificationMailer(host, port, fromAddress, username, password, enableSsl));

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
