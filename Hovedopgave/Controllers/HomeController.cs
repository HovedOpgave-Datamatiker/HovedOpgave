using Hovedopgave.Data;
using Hovedopgave.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using log4net;

namespace Hovedopgave.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HovedopgaveContext _context;

        // Add log4net logger
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
                logger.Warn("Home: No username found, returning Unauthorized");
                return Unauthorized();
            }

            var currentUser = await _context.User
                .FirstOrDefaultAsync(u => u.Username == username);

            if (currentUser == null)
            {
                logger.Warn($"Home: No user found for username '{username}', returning Unauthorized");
                return Unauthorized();
            }

            logger.Info($"Home: Found user '{username}', fetching assigned tickets");
            var assignedTickets = await _context.Ticket
                .Where(t => t.Users.Any(u => u.Id == currentUser.Id))
                .Where(t => t.IsFinished == false)
                .Include(t => t.Users)
                .Include(t => t.Station)
                .OrderByDescending(t => t.Priority)
                .ToListAsync();

            TicketsController.Return = "MyTickets";
            logger.Info($"Home: Returning {assignedTickets.Count} tickets for user '{username}'");
            return View(assignedTickets);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            logger.Error("Home: Error action called");
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
