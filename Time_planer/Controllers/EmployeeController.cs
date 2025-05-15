using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Time_planer.Data;
using Time_planer.Models;

namespace Time_planer.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;

        public EmployeeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(DateOnly? start = null, DateOnly? end = null)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var workTimes = _context.WorkTimeEntries
                .Where(e => e.UserId == userId)
                .AsQueryable();

            if (start != null && end != null)
            {
                workTimes = workTimes.Where(w =>
                    w.StartTime.Date >= start.Value.ToDateTime(TimeOnly.MinValue) &&
                    w.StartTime.Date <= end.Value.ToDateTime(TimeOnly.MaxValue));
            }

            var workTimesList = workTimes.OrderByDescending(e => e.StartTime).ToList();

            var totalHours = workTimesList
                .Where(e => e.TotalHours.HasValue)
                .Sum(e => e.TotalHours.Value);

            var leaveRequests = _context.LeaveRequests
                .Where(l => l.UserId == userId)
                .OrderByDescending(l => l.StartDate)
                .ToList();

            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.TotalHours = totalHours;

            var model = new EmployeeDashboardViewModel
            {
                WorkTimes = workTimesList,
                LeaveRequests = leaveRequests
            };
            var currentYear = DateTime.Now.Year;

            var usedDays = _context.LeaveRequests
                .Where(l => l.UserId == userId && l.Status == "Одобрено" && l.StartDate.Year == currentYear)
                .Sum(l => EF.Functions.DateDiffDay(l.StartDate, l.EndDate) + 1);

            var remainingDays = Math.Max(0, 28 - usedDays);

            ViewBag.RemainingDays = remainingDays;

            return View(model);
        }

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

    }
}
