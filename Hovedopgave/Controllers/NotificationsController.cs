using Hovedopgave.Data;
using Hovedopgave.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using log4net;

namespace Hovedopgave.Controllers
{
    [Authorize]
    public class NotificationsController : Controller
    {
        private readonly HovedopgaveContext _context;
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public NotificationsController(HovedopgaveContext context)
        {
            _context = context;
        }

        // GET: Notifications/Edit
        public async Task<IActionResult> Edit()
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
            {
                logger.Warn("No username found in Notifications/Edit GET, returning Unauthorized");
                return Unauthorized();
            }

            var currentUser = await _context.User.FirstOrDefaultAsync(u => u.Username == username);
            if (currentUser == null)
            {
                logger.Warn($"No user found for username '{username}' in Notifications/Edit GET, returning Unauthorized");
                return Unauthorized();
            }

            var setting = await _context.NotificationSetting.FirstOrDefaultAsync(n => n.UserId == currentUser.Id);
            if (setting == null)
            {
                setting = new NotificationSetting
                {
                    UserId = currentUser.Id,
                    EmailNotificationsEnabled = true,
                    Frequency = NotificationFrequency.Always
                };
                _context.NotificationSetting.Add(setting);
                await _context.SaveChangesAsync();
            }

            return View(setting);
        }

        // POST: Notifications/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,EmailNotificationsEnabled,Frequency")] NotificationSetting postedSetting)
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
            {
                logger.Warn("No username found in Notifications/Edit POST, returning Unauthorized");
                return Unauthorized();
            }

            var currentUser = await _context.User.FirstOrDefaultAsync(u => u.Username == username);
            if (currentUser == null)
            {
                logger.Warn($"No user found for username '{username}' in Notifications/Edit POST, returning Unauthorized");
                return Unauthorized();
            }

            var setting = await _context.NotificationSetting
                .FirstOrDefaultAsync(n => n.Id == postedSetting.Id && n.UserId == currentUser.Id);

            if (setting == null)
            {
                logger.Warn($"No notification setting found for id={postedSetting.Id} in Notifications/Edit POST");
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(setting);
            }

            setting.EmailNotificationsEnabled = postedSetting.EmailNotificationsEnabled;
            setting.Frequency = postedSetting.Frequency;

            _context.Update(setting);
            await _context.SaveChangesAsync();
            logger.Info($"Updated notification setting for user '{username}'");

            return RedirectToAction(nameof(Edit));
        }
    }
}
