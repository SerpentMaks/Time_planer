using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Time_planer.Data;
using Time_planer.Models;

namespace Time_planer.Controllers
{
    public class ManagerController : Controller
    {
        private readonly AppDbContext _context;

        public ManagerController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(DateOnly? start = null, DateOnly? end = null, string searchName = null, string filter = "new")

        {
            var yearStart = new DateTime(DateTime.Now.Year, 1, 1); 

            var users = _context.Users.Include(u => u.Role)
                          .Where(u => u.Role.RoleName == "Сотрудник")
                          .AsQueryable();

            if (!string.IsNullOrEmpty(searchName))
            {
                users = users.Where(u => (u.FirstName + " " + u.LastName).Contains(searchName));
            }

            var workEntries = _context.WorkTimeEntries
                .Include(w => w.User)
                .Where(w => w.TotalHours != null)
                .AsQueryable(); 

            var stats = users.ToList().Select(user =>
            {
                var entries = workEntries.Where(e => e.UserId == user.UserId);

                return new EmployeeTimeStats
                {
                    UserId = user.UserId,
                    Name = $"{user.FirstName} {user.LastName}",
                    TotalHours = entries.Sum(e => e.TotalHours ?? 0),
                    ThisWeekHours = entries
                        .Where(e => e.StartTime >= DateTime.Now.AddDays(-7))
                        .Sum(e => e.TotalHours ?? 0),
                    ThisMonthHours = entries
                        .Where(e => e.StartTime >= DateTime.Now.AddMonths(-1))
                        .Sum(e => e.TotalHours ?? 0),
                    ThisYearHours = entries
                        .Where(e => e.StartTime >= yearStart) 
                        .Sum(e => e.TotalHours ?? 0),
                    CustomPeriodHours = (start != null && end != null)
                        ? entries.Where(e =>
                              e.StartTime.Date >= start.Value.ToDateTime(TimeOnly.MinValue) &&
                              e.StartTime.Date <= end.Value.ToDateTime(TimeOnly.MaxValue))
                              .Sum(e => e.TotalHours ?? 0)
                        : 0
                };
            }).ToList();

            var currentYear = DateTime.Now.Year;
            var leaveRequestsQuery = _context.LeaveRequests.Include(l => l.User).AsQueryable();

            if (filter == "new")
            {
                leaveRequestsQuery = leaveRequestsQuery.Where(l => l.Status == "Ожидает");
            }
            else if (filter == "processed")
            {
                leaveRequestsQuery = leaveRequestsQuery.Where(l => l.Status != "Ожидает");
            }

            var leaveRequests = leaveRequestsQuery.ToList();


            var leaveWithBalance = leaveRequests.Select(leave =>
            {
                var usedDays = _context.LeaveRequests
                    .Where(l =>
                        l.UserId == leave.UserId &&
                        l.Status == "Одобрено" &&
                        l.StartDate.Year == currentYear)
                    .Sum(l => EF.Functions.DateDiffDay(l.StartDate, l.EndDate) + 1);

                var requestedDays = (leave.EndDate.ToDateTime(TimeOnly.MinValue) - leave.StartDate.ToDateTime(TimeOnly.MinValue)).Days + 1;

                return new LeaveRequestWithBalance
                {
                    Leave = leave,
                    RemainingDays = Math.Max(0, 28 - usedDays),
                    RequestedDays = requestedDays
                };
            })
.OrderByDescending(x => x.Leave.Status == "Ожидает")
.ThenByDescending(x => x.Leave.StartDate)
.ToList();




            return View(new ManagerDashboardViewModel
            {
                TimeStats = stats,
                LeaveRequests = leaveWithBalance,
                CustomStart = start,
                CustomEnd = end
            });
        }
        public IActionResult Report(DateOnly? start = null, DateOnly? end = null)
        {
            var users = _context.Users
                .Include(u => u.Role)
                .Where(u => u.Role.RoleName == "Сотрудник")
                .ToList();

            var startDate = start?.ToDateTime(TimeOnly.MinValue);
            var endDate = end?.ToDateTime(TimeOnly.MaxValue);

            var stats = new List<EmployeeTimeStats>();

            foreach (var user in users)
            {
                var entries = _context.WorkTimeEntries
                    .Where(e => e.UserId == user.UserId);

                if (startDate != null && endDate != null)
                {
                    entries = entries.Where(e => e.StartTime >= startDate && e.StartTime <= endDate);
                }

                var entryList = entries.ToList();
                var total = entryList.Sum(e => e.TotalHours ?? 0);
                var days = entryList.Count;
                var avg = days > 0 ? total / days : 0;

                stats.Add(new EmployeeTimeStats
                {
                    UserId = user.UserId,
                    Name = user.FirstName + " " + user.LastName,
                    TotalHours = total,
                    Days = days,
                    AvgDay = avg
                });
            }

            ViewBag.Start = start;
            ViewBag.End = end;

            return View(stats);
        }

        public IActionResult EmployeeDetails(int userId, DateOnly? start = null, DateOnly? end = null)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null) return NotFound();

            var entries = _context.WorkTimeEntries
                .Where(e => e.UserId == userId)
                .AsQueryable();

            if (start != null && end != null)
            {
                var startDate = start.Value.ToDateTime(TimeOnly.MinValue);
                var endDate = end.Value.ToDateTime(TimeOnly.MaxValue);
                entries = entries.Where(e => e.StartTime >= startDate && e.StartTime <= endDate);
            }

            var entryList = entries.OrderBy(e => e.StartTime).ToList();

            var totalHours = entryList.Sum(e => e.TotalHours ?? 0);
            var workDays = entryList.Count;
            var avgDay = workDays > 0 ? totalHours / workDays : 0;

            ViewBag.UserId = user.UserId;
            ViewBag.UserName = user.FirstName + " " + user.LastName;
            ViewBag.TotalHours = totalHours;
            ViewBag.DaysWorked = workDays;
            ViewBag.AvgPerDay = avgDay;
            ViewBag.Start = start;
            ViewBag.End = end;

            return View(entryList);
        }


        [HttpPost]
        public IActionResult Approve(int leaveId)
        {
            var request = _context.LeaveRequests.Find(leaveId);
            if (request != null)
            {
                request.Status = "Одобрено";
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Reject(int leaveId)
        {
            var request = _context.LeaveRequests.Find(leaveId);
            if (request != null)
            {
                request.Status = "Отклонено";
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
