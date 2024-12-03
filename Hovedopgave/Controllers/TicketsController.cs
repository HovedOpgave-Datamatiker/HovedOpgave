using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hovedopgave.Data;
using Hovedopgave.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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
            // Replace this with the actual logic to get the logged-in user's ID
            int currentUserId = _context.User.Where(U => U.Username == User.Identity.Name).FirstOrDefault().Id;

            return View(await _context.Ticket.Where(t => t.UserId == currentUserId).ToListAsync());
        }

        // GET: Open Tickets
        public async Task<IActionResult> Index()
        {
            return View(await _context.Ticket.Where(t => t.IsFinished == false).OrderByDescending(t => t.Priority).ToListAsync());
        }

        // GET: All Tickets
        public async Task<IActionResult> OpenTickets()
        {
            return View(await _context.Ticket.ToListAsync());
        }

        // GET: Closed Tickets
        public async Task<IActionResult> ClosedTickets()
        {
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
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,IsFinished,Created,LastUpdated,Priority,UserId,User")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,IsFinished,Created,LastUpdated,Priority")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
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
                if(ticket.IsFinished)
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
