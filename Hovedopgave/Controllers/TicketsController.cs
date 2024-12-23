using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hovedopgave.Data;
using Hovedopgave.Models;
using Microsoft.AspNetCore.Authorization;
using Hovedopgave.Services;
using System.Diagnostics;
using log4net; 

namespace Hovedopgave.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private readonly HovedopgaveContext _context;
        private readonly NotificationMailer _mailer;
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType); 
        public static String? Return { get; set; }

        public TicketsController(HovedopgaveContext context, NotificationMailer mailer)
        {
            _context = context;
            _mailer = mailer;
        }

        //GET: User Tickets
        public async Task<IActionResult> MyTickets()
        {
            Return = "MyTickets";

            int currentUserId = _context.User
                .Where(u => u.Username == User.Identity.Name)
                .Select(u => u.Id)
                .FirstOrDefault();

            return View(await _context.Ticket
                .Include(t => t.Station)
                .Include(t => t.Users)
                .Where(t => t.Users.Any(u => u.Id == currentUserId))
                .ToListAsync());
        }

        // GET: Open Tickets
        public async Task<IActionResult> Index()
        {
            Return = "Index";

            return View(await _context.Ticket
                .Include(t => t.Station)
                .Where(t => t.IsFinished == false)
                .OrderByDescending(t => t.Priority)
                .ToListAsync());
        }

        // GET: All Tickets
        public async Task<IActionResult> OpenTickets()
        {
            Return = "Index";
            return View(await _context.Ticket.ToListAsync());
        }

        // GET: Closed Tickets
        public async Task<IActionResult> ClosedTickets()
        {
            Return = "ClosedTickets";
            return View(await _context.Ticket
                .Include(t => t.Station)
                .Where(t => t.IsFinished == true)
                .ToListAsync());
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket
                .Include(t => t.Users)
                .Include(t => t.Station)
                .Include(t => t.Comments)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            ViewBag.Return = Return;

            return View(ticket);
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(int id, string comment)
        {

            if (string.IsNullOrWhiteSpace(comment))
            {
                ModelState.AddModelError("Comment", "Kommentar kan ikke være tom.");
                return RedirectToAction("Details", new { id });
            }


            var newComment = new Comment
            {
                TicketId = id,
                Created = DateTime.Now,
                CreatedBy = User.Identity.Name,
                Text = comment
            };

            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket == null)
            {
                logger.Warn($"AddComment failed: No ticket found with id={id}"); 
                return NotFound();
            }
            ticket.LastUpdated = DateTime.Now;
            ticket.LastUpdatedBy = User.Identity.Name;
            _context.Update(ticket);

            _context.Comments.Add(newComment);
            await _context.SaveChangesAsync();

            ViewBag.Return = Return;
            return RedirectToAction(nameof(Details), new { id });
        }

        // GET: Tickets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ticket ticket, int[] selectedUserIds, int selectedStationId)
        {
            if (ModelState.IsValid)
            {
                if (selectedUserIds != null)
                {
                    foreach (var userId in selectedUserIds)
                    {
                        var user = await _context.User.FindAsync(userId);
                        if (user != null)
                        {
                            ticket.Users.Add(user);
                        }
                    }
                }


                ticket.CreatedBy = User.Identity.Name;
                ticket.LastUpdatedBy = User.Identity.Name;
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                logger.Info($"Ticket {ticket.Id} created by {User.Identity.Name}"); 

                // Notify Users
                await NotifyUsersAboutTicketCreation(ticket);

                return RedirectToAction(Return);
            }

            ViewBag.Users = new SelectList(_context.User, "Id", "FullName");
            ViewBag.Return = Return;
            return View(ticket);
        }

        public IActionResult Create()
        {
            ViewBag.Users = new SelectList(_context.User, "Id", "FullName");
            ViewBag.Stations = new SelectList(_context.Station, "Id", "Name");
            ViewBag.Return = Return;
            return View();
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket
                .Include(t => t.Users)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (ticket == null)
            {
                return NotFound();
            }

            ticket.SelectedUserIds = ticket.Users.Select(u => u.Id).ToArray();
            ViewBag.Users = new SelectList(_context.User, "Id", "FullName");

            ViewBag.Return = Return;


            return View(ticket);
        }

        // POST: Tickets/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var ticketToUpdate = await _context.Ticket
                        .Include(t => t.Users)
                        .FirstOrDefaultAsync(t => t.Id == ticket.Id);

                    if (ticketToUpdate == null)
                    {
                        logger.Warn($"Edit failed: No ticket found with id={ticket.Id}"); 
                        return NotFound();
                    }

                    ticketToUpdate.Description = ticket.Description;
                    ticketToUpdate.Priority = ticket.Priority;
                    ticketToUpdate.LastUpdated = DateTime.Now;
                    ticketToUpdate.LastUpdatedBy = User.Identity.Name;

                    ticketToUpdate.Users.Clear();
                    if (ticket.SelectedUserIds != null)
                    {
                        foreach (var userId in ticket.SelectedUserIds)
                        {
                            var user = await _context.User.FindAsync(userId);
                            if (user != null)
                            {
                                ticketToUpdate.Users.Add(user);
                            }
                        }
                    }

                    _context.Update(ticketToUpdate);
                    await _context.SaveChangesAsync();
                    logger.Info($"Ticket {ticket.Id} updated by {User.Identity.Name}"); 

                    // Notify users update
                    await NotifyUsersAboutTicketUpdate(ticketToUpdate);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(Return);
            }

            ViewBag.Users = new SelectList(_context.User, "Id", "FullName");
            ViewBag.Return = Return;
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            ViewBag.Return = Return;
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket != null)
            {
                _context.Ticket.Remove(ticket);
            }

            await _context.SaveChangesAsync();
            logger.Info($"Ticket {id} deleted by {User.Identity.Name}"); 
            return RedirectToAction(Return);
        }

        private bool TicketExists(int id)
        {
            return _context.Ticket.Any(e => e.Id == id);
        }

        public async Task<IActionResult> ChangeStatus(int id)
        {
            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket != null)
            {
                ticket.IsFinished = !ticket.IsFinished;
                ticket.LastUpdated = DateTime.Now;
                _context.Update(ticket);
                await _context.SaveChangesAsync();
                logger.Info($"Ticket {id} status changed by {User.Identity.Name}"); 
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task NotifyUsersAboutTicketCreation(Ticket ticket)
        {
            _mailer.ClearRecipients();

            var assignedUsers = ticket.Users.ToList();
            foreach (var user in assignedUsers)
            {
                var setting = await _context.NotificationSetting.FirstOrDefaultAsync(n => n.UserId == user.Id);
                if (setting != null && setting.EmailNotificationsEnabled && setting.Frequency == NotificationFrequency.Always)
                {
                    _mailer.AddRecipient(user.Email);
                }
            }

            _mailer.SendTicketCreatedNotification(ticket);
        }

        private async Task NotifyUsersAboutTicketUpdate(Ticket ticket)
        {
            _mailer.ClearRecipients();

            var assignedUsers = ticket.Users.ToList();
            foreach (var user in assignedUsers)
            {
                var setting = await _context.NotificationSetting.FirstOrDefaultAsync(n => n.UserId == user.Id);
                if (setting != null && setting.EmailNotificationsEnabled && setting.Frequency == NotificationFrequency.Always)
                {
                    _mailer.AddRecipient(user.Email);
                }
            }

            _mailer.SendTicketUpdatedNotification(ticket);
        }
    }
}
