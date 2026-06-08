using CampusEventsApp.Data;
using CampusEventsApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CampusEventsApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        //  DASHBOARD
        public async Task<IActionResult> Dashboard()
        {
            ViewBag.TotalEvents = await _context.Events.CountAsync();
            ViewBag.TotalRegistrations = await _context.EventRegistrations.CountAsync();
            return View();
        }

        // ADD EVENT
        [HttpGet]
        public IActionResult AddEvent() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEvent(Event model)
        {
            if (ModelState.IsValid)
            {
                _context.Events.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ManageEvents));
            }
            return View(model);
        }

        // MANAGE EVENTS
        public async Task<IActionResult> ManageEvents()
        {
            var events = await _context.Events
                .OrderByDescending(e => e.EventDate)
                .ToListAsync();

            return View(events);
        }

        // TOGGLE STATUS
        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var ev = await _context.Events.FindAsync(id);
            if (ev != null)
            {
                ev.IsOpen = !ev.IsOpen;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(ManageEvents));
        }

        // DELETE EVENT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var ev = await _context.Events.FindAsync(id);
            if (ev != null)
            {
                _context.Events.Remove(ev);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(ManageEvents));
        }

        // EDIT EVENT
        [HttpGet]
        public async Task<IActionResult> EditEvent(int id)
        {
            var ev = await _context.Events.FindAsync(id);
            if (ev == null) return NotFound();
            return View(ev);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEvent(Event model)
        {
            if (ModelState.IsValid)
            {
                _context.Events.Update(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ManageEvents));
            }
            return View(model);
        }

        // PARTICIPANTS
        public async Task<IActionResult> Participants(int id)
        {
            var eventDetails = await _context.Events
                .Include(e => e.Registrations)
                .ThenInclude(r => r.Student)
                .FirstOrDefaultAsync(e => e.Id == id);

            return View(eventDetails);
        }
        public async Task<IActionResult> Users()
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }
    }
}