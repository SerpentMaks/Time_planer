using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Time_planer.Data;
using Time_planer.Models;

namespace Time_planer.Controllers
{
    public class RegistrarController : Controller
    {
        private readonly AppDbContext _context;

        public RegistrarController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string searchName = null)
        {
            var employees = _context.Users
                .Include(u => u.Role)
                .Where(u => u.Role.RoleName == "Сотрудник")
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchName))
            {
                employees = employees.Where(u => (u.FirstName + " " + u.LastName).Contains(searchName));
            }

            

        var activeEntries = _context.WorkTimeEntries
    .Where(e => e.EndTime == null && e.UserId != null)
    .ToDictionary(e => e.UserId.Value, e => e.StartTime);
            ViewBag.ActiveEntries = activeEntries;

            return View(employees.ToList());
        }

        [HttpPost]
        public IActionResult StartWork(int userId)
        {
            var registrarId = HttpContext.Session.GetInt32("UserId");
            if (registrarId == null) return RedirectToAction("Login", "Account");

            var existingEntry = _context.WorkTimeEntries
                .FirstOrDefault(e => e.UserId == userId && e.EndTime == null);

            if (existingEntry == null)
            {
                var entry = new WorkTimeEntry
                {
                    UserId = userId,
                    RegistrarId = registrarId.Value,
                    StartTime = DateTime.Now
                };
                _context.WorkTimeEntries.Add(entry);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult EndWork(int userId)
        {
            var entry = _context.WorkTimeEntries
                .FirstOrDefault(e => e.UserId == userId && e.EndTime == null);

            if (entry != null)
            {
                entry.EndTime = DateTime.Now;
                entry.TotalHours = (decimal)(entry.EndTime.Value - entry.StartTime).TotalHours;
                _context.SaveChanges();

                var user = _context.Users.Find(userId);
                TempData["Notification"] = $"Сотрудник {user.FirstName} {user.LastName} отработал {entry.TotalHours:F2} часов.";
            }

            return RedirectToAction("Index");
        }
    }
}
