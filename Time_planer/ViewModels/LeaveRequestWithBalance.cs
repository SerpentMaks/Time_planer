using Time_planer.Models;

public class LeaveRequestWithBalance
{
    public LeaveRequest Leave { get; set; }
    public int RemainingDays { get; set; }
    public int RequestedDays { get; set; }
}
