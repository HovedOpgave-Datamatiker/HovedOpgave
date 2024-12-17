using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hovedopgave.Data;
using Hovedopgave.Models;
using Microsoft.AspNetCore.Authorization;

namespace Hovedopgave.Controllers
{
    [Authorize]
    public class NotificationsController : Controller
    {
        private readonly HovedopgaveContext _context;

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
                return Unauthorized();
            }

            var currentUser = await _context.User
                .FirstOrDefaultAsync(u => u.Username == username);

            if (currentUser == null)
            {
                return Unauthorized();
            }

            var setting = await _context.NotificationSetting
                .FirstOrDefaultAsync(n => n.UserId == currentUser.Id);

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
                return Unauthorized();
            }

            var currentUser = await _context.User
                .FirstOrDefaultAsync(u => u.Username == username);

            if (currentUser == null)
            {
                return Unauthorized();
            }

            var setting = await _context.NotificationSetting
                .FirstOrDefaultAsync(n => n.Id == postedSetting.Id && n.UserId == currentUser.Id);

            if (setting == null)
            {
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

            return RedirectToAction(nameof(Edit));
        }
    }
}
