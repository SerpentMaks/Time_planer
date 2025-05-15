using System.Collections.Generic;

namespace Time_planer.Models
{
    public class EmployeeDashboardViewModel
    {
        public List<WorkTimeEntry> WorkTimes { get; set; }
        public List<LeaveRequest> LeaveRequests { get; set; }
    }
}
