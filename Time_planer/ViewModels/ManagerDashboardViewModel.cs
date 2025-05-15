using System;
using System.Collections.Generic;

namespace Time_planer.Models
{
    public class EmployeeTimeStats
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public decimal TotalHours { get; set; }
        public decimal ThisWeekHours { get; set; }
        public decimal ThisMonthHours { get; set; }
        public decimal ThisYearHours { get; set; }
        public decimal CustomPeriodHours { get; set; }
        public int Days { get; set; }
        public decimal AvgDay { get; set; }
    }

    public class ManagerDashboardViewModel
    {
       
            public List<EmployeeTimeStats> TimeStats { get; set; }
            public List<LeaveRequestWithBalance> LeaveRequests { get; set; }
            public DateOnly? CustomStart { get; set; }
            public DateOnly? CustomEnd { get; set; }
        

    }
}
