﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hovedopgave.Data;
using Hovedopgave.Models;
using Microsoft.AspNetCore.Authorization;
using Hovedopgave.Services;

namespace Hovedopgave.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private readonly HovedopgaveContext _context;
        private readonly NotificationMailer _mailer;

        public TicketsController(HovedopgaveContext context, NotificationMailer mailer)
        {
            _context = context;
            _mailer = mailer;
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

                // Notify Users
                await NotifyUsersAboutTicketCreation(ticket);

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
                ticket.IsFinished = !ticket.IsFinished;
                ticket.LastUpdated = DateTime.Now;
                _context.Update(ticket);
                await _context.SaveChangesAsync();
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
