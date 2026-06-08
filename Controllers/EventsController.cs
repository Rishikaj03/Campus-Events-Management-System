using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CampusEventsApp.Data;
using CampusEventsApp.Models;

namespace CampusEventsApp.Controllers
{
    // REMOVED the Admin lock from up here so students can access the controller!
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // NEW: VIEW EVENTS (For Everyone)
        // [Authorize] just means they must be logged in, but any role (Student or Admin) is fine.
        [Authorize]
        public IActionResult Index()
        {
            // Fetch all events and send them to the view
            var events = _context.Events.ToList();
            return View(events);
        }

        // --------------------------------------------------------
        //  ADMIN ONLY ACTIONS BELOW
        // --------------------------------------------------------

        // 1. MANAGE EVENTS (Admin Only)
        [Authorize(Roles = "Admin")] // Admin lock moved here
        public IActionResult ManageEvents()
        {
            var events = _context.Events.ToList();
            return View(events);
        }

        // 2. ADD EVENT (Show Form - Admin Only)
        [Authorize(Roles = "Admin")] // Admin lock moved here
        [HttpGet]
        public IActionResult AddEvent()
        {
            return View();
        }

        // 3. ADD EVENT (Save - Admin Only)
        [Authorize(Roles = "Admin")] // Admin lock moved here
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddEvent(Event newEvent)
        {
            if (ModelState.IsValid)
            {
                _context.Events.Add(newEvent);
                _context.SaveChanges();
                return RedirectToAction("ManageEvents");
            }
            return View(newEvent);
        }

        // 4. REMOVE EVENT (Admin Only)
        [Authorize(Roles = "Admin")] // Admin lock moved here
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteEvent(int id)
        {
            var eventToDelete = _context.Events.Find(id);
            if (eventToDelete != null)
            {
                _context.Events.Remove(eventToDelete);
                _context.SaveChanges();
            }
            return RedirectToAction("ManageEvents");
        }
    }
}