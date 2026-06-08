using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CampusEventsApp.Data;
using CampusEventsApp.Models;

namespace CampusEventsApp.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public StudentController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // DASHBOARD
        public async Task<IActionResult> Dashboard()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                if (user.Email != "admin@campus.com")
                {
                    var isStudent = await _userManager.IsInRoleAsync(user, "Student");
                    var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

                    if (!isStudent && !isAdmin)
                    {
                        await _userManager.AddToRoleAsync(user, "Student");
                    }
                }

                ViewBag.TotalEvents = await _context.Events.CountAsync(e => e.IsOpen);
                ViewBag.MyRegistrations = await _context.EventRegistrations
                    .CountAsync(r => r.StudentId == user.Id);
            }
            else
            {
                ViewBag.TotalEvents = 0;
                ViewBag.MyRegistrations = 0;
            }

            return View();
        }

        // EVENT LIST
        public async Task<IActionResult> EventList()
        {
            var today = DateTime.Today;

            var events = await _context.Events
                .Include(e => e.Registrations)
                .Where(e =>
                    e.EventDate.Date >= today &&
                    e.IsOpen
                )
                .OrderBy(e => e.EventDate)
                .ToListAsync();

            return View(events);
        }

        // EVENT DETAILS
        public async Task<IActionResult> EventDetails(int id)
        {
            var ev = await _context.Events
                .Include(e => e.Registrations)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (ev == null)
                return NotFound();

            return View(ev);
        }

        //  REGISTER 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(int eventId)
        {
            var userId = _userManager.GetUserId(User);

            var ev = await _context.Events
                .Include(e => e.Registrations)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (ev == null)
                return RedirectToAction(nameof(EventList));

            //  ALREADY REGISTERED CHECK 
            var alreadyRegistered = await _context.EventRegistrations
                .AnyAsync(r => r.EventId == eventId && r.StudentId == userId);

            if (alreadyRegistered)
            {
                TempData["Message"] = "You can't register again. You are already registered for this event.";
                return RedirectToAction(nameof(EventDetails), new { id = eventId });
            }

            if (ev.IsOpen && ev.CapacityLeft > 0)
            {
                _context.EventRegistrations.Add(new EventRegistration
                {
                    EventId = eventId,
                    StudentId = userId
                });

                await _context.SaveChangesAsync();

                // CLOSE EVENT IF FULL
                if (ev.CapacityLeft <= 0)
                {
                    ev.IsOpen = false;
                    await _context.SaveChangesAsync();
                }

                TempData["Message"] = "Successfully registered!";
                return RedirectToAction(nameof(RegistrationSuccess));
            }

            TempData["Message"] = "Registration is not available.";
            return RedirectToAction(nameof(EventList));
        }

        // SUCCESS PAGE
        public IActionResult RegistrationSuccess()
        {
            return View();
        }

        // MY EVENTS
        public async Task<IActionResult> MyEvents()
        {
            var userId = _userManager.GetUserId(User);

            var myEvents = await _context.EventRegistrations
                .Include(r => r.Event)
                .Where(r => r.StudentId == userId)
                .ToListAsync();

            return View(myEvents);
        }

        // CANCEL REGISTRATION
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelRegistration(int eventId)
        {
            var userId = _userManager.GetUserId(User);

            var registration = await _context.EventRegistrations
                .FirstOrDefaultAsync(r => r.EventId == eventId && r.StudentId == userId);

            if (registration != null)
            {
                _context.EventRegistrations.Remove(registration);
                await _context.SaveChangesAsync();

                var ev = await _context.Events.FindAsync(eventId);
                if (ev != null && ev.CapacityLeft > 0 && ev.EventDate.Date >= DateTime.Today)
                {
                    ev.IsOpen = true;
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction(nameof(MyEvents));
        }
    }
}