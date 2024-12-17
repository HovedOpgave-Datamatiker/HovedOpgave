using Hovedopgave.Data;
using Hovedopgave.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace Hovedopgave.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HovedopgaveContext _context;

        public HomeController(ILogger<HomeController> logger, HovedopgaveContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Index()
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

            var assignedTickets = await _context.Ticket
                .Where(t => t.Users.Any(u => u.Id == currentUser.Id))
                .Include(t => t.Users)
                .OrderByDescending(t => t.Priority)
                .ToListAsync();

            return View(assignedTickets);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
