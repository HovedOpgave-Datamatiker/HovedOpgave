using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hovedopgave.Data;
using Hovedopgave.Models;
using Microsoft.AspNetCore.Authorization;

namespace Hovedopgave.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private readonly HovedopgaveContext _context;

        public TicketsController(HovedopgaveContext context)
        {
            _context = context;
        }

        //GET: User Tickets
        public async Task<IActionResult> MyTickets()
        {
            TempData["Return"] = "MyTickets";

            int currentUserId = _context.User.Where(U => U.Username == User.Identity.Name).FirstOrDefault().Id;

            return View(await _context.Ticket.Where(t => t.UserId == currentUserId).ToListAsync());
        }

        // GET: Open Tickets
        public async Task<IActionResult> Index()
        {
            TempData["Return"] = "Index";

            return View(await _context.Ticket.Where(t => t.IsFinished == false).OrderByDescending(t => t.Priority).ToListAsync());
        }

        // GET: All Tickets
        public async Task<IActionResult> OpenTickets()
        {
            TempData["Return"] = "Index";

            return View(await _context.Ticket.ToListAsync());
        }

        // GET: Closed Tickets
        public async Task<IActionResult> ClosedTickets()
        {
            TempData["Return"] = "ClosedTickets";
            return View(await _context.Ticket.Where(t => t.IsFinished == true).ToListAsync());
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
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }


            ViewBag.Return = TempData["Return"];

            TempData["Return"] = "Index";


            return View(ticket);
        }

        // GET: Tickets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ticket ticket, int[] selectedUserIds)
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

                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Users = new SelectList(_context.User, "Id", "FullName");
            return View(ticket);
        }

        public IActionResult Create()
        {
            ViewBag.Users = new SelectList(_context.User, "Id", "FullName");
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
                        return NotFound();
                    }

                    ticketToUpdate.Description = ticket.Description;
                    ticketToUpdate.Priority = ticket.Priority;
                    ticketToUpdate.LastUpdated = DateTime.Now;
                    ticketToUpdate.CreatedBy = User.Identity.Name;

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
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Users = new SelectList(_context.User, "Id", "FullName");
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
            return RedirectToAction(nameof(Index));
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
                if (ticket.IsFinished)
                {
                    ticket.IsFinished = false;
                }
                else
                {
                    ticket.IsFinished = true;
                }
                ticket.LastUpdated = DateTime.Now;
                _context.Update(ticket);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
