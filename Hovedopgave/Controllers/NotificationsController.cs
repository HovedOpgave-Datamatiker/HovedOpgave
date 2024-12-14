using System.Linq;
using System.Threading.Tasks;
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
            var currentUser = await _context.User
                .FirstOrDefaultAsync(u => u.FullName == User.Identity.Name);

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
                    EmailNotificationsEnabled = false,
                    Frequency = NotificationFrequency.Always
                };

                _context.NotificationSetting.Add(setting);
                await _context.SaveChangesAsync();
            }

            return View(setting);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(NotificationSetting postedSetting)
        {
            var currentUser = await _context.User
                .FirstOrDefaultAsync(u => u.FullName == User.Identity.Name);

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

            if (ModelState.IsValid)
            {
                try
                {
                    setting.EmailNotificationsEnabled = postedSetting.EmailNotificationsEnabled;
                    setting.Frequency = postedSetting.Frequency;

                    _context.Update(setting);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotificationSettingExists(setting.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Edit));
            }

            return View(setting);
        }

        private bool NotificationSettingExists(int id)
        {
            return _context.NotificationSetting.Any(e => e.Id == id);
        }
    }
}
