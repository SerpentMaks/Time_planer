using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Time_planer.Data;
using Time_planer.Models;

namespace Time_planer.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user != null)
            {
                HttpContext.Session.SetInt32("UserId", user.UserId);
                HttpContext.Session.SetString("Role", user.Role.RoleName);
                HttpContext.Session.SetString("UserName", user.FirstName);

                switch (user.Role.RoleName)
                {
                    case "Сотрудник":
                        return RedirectToAction("Index", "Employee");
                    case "Регистратор":
                        return RedirectToAction("Index", "Registrar");
                    case "Менеджер":
                        return RedirectToAction("Index", "Manager");
                    case "Администратор":
                        return RedirectToAction("Index", "Admin");
                    default:
                        return RedirectToAction("Login");
                }
            }

            ViewBag.Error = "Неверный email или пароль";
            return View();
        }

        [HttpPost]
        [HttpPost]
        public IActionResult RequestLeave(string leaveType, DateTime startDate, DateTime endDate)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var request = new LeaveRequest
            {
                UserId = userId.Value,
                LeaveType = leaveType,
                StartDate = DateOnly.FromDateTime(startDate),
                EndDate = DateOnly.FromDateTime(endDate),
                Status = "Ожидает"
            };

            _context.LeaveRequests.Add(request);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
