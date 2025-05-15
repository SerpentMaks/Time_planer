using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Time_planer.Data;
using Time_planer.Models;

namespace Time_planer.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string searchUser)
        {
            var users = _context.Users.Include(u => u.Role).AsQueryable();

            if (!string.IsNullOrEmpty(searchUser))
            {
                users = users.Where(u => (u.FirstName + " " + u.LastName).Contains(searchUser));
            }
            ViewBag.Roles = _context.Roles.ToList();

            return View(users.ToList());
        }


        [HttpPost]
        public IActionResult ChangeRole(int userId, int roleId)
        {
            var user = _context.Users.Find(userId);
            if (user != null)
            {
                user.RoleId = roleId;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult AddUser(string firstName, string lastName, string email, string password, int roleId)
        {
            var user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = password,
                RoleId = roleId
            };

            _context.Users.Add(user);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteUser(int userId)
        {
            var user = _context.Users.Find(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult WorkTime(string searchName, DateTime? searchDate)
        {
            var records = _context.WorkTimeEntries
                .Include(w => w.User)
                .Include(w => w.Registrar)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchName))
            {
                records = records.Where(w =>
                    (w.User.FirstName + " " + w.User.LastName).Contains(searchName) ||
                    (w.Registrar.FirstName + " " + w.Registrar.LastName).Contains(searchName)
                );
            }

            if (searchDate.HasValue)
            {
                records = records.Where(w => w.StartTime.Date == searchDate.Value.Date);
            }

            var result = records.OrderByDescending(w => w.StartTime).ToList();
            return View(result);
        }

        [HttpPost]
        [HttpPost]
        public IActionResult EditWorkTime(int entryId, DateTime startTime, DateTime endTime)
        {
            var entry = _context.WorkTimeEntries.Find(entryId);
            if (entry != null)
            {
                if (startTime >= endTime)
                {
                    TempData["Error"] = "Ошибка: Время начала должно быть раньше времени окончания!";
                    return RedirectToAction("WorkTime");
                }

                entry.StartTime = startTime;
                entry.EndTime = endTime;
                entry.TotalHours = (decimal)(endTime - startTime).TotalHours;
                _context.SaveChanges();
            }
            return RedirectToAction("WorkTime");
        }


        [HttpPost]
        public IActionResult DeleteWorkTime(int entryId)
        {
            var entry = _context.WorkTimeEntries.Find(entryId);
            if (entry != null)
            {
                _context.WorkTimeEntries.Remove(entry);
                _context.SaveChanges();
            }
            return RedirectToAction("WorkTime");
        }
    }
}
