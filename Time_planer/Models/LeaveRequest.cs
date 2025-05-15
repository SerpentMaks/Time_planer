using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Time_planer.Models;

[Table("leave_requests")]
public partial class LeaveRequest
{
    [Key]
    [Column("leave_id")]
    public int LeaveId { get; set; }

    [Column("user_id")]
    public int? UserId { get; set; }

    [Column("leave_type")]
    [StringLength(100)]
    [Unicode(false)]
    public string LeaveType { get; set; } = null!;

    [Column("start_date")]
    public DateOnly StartDate { get; set; }

    [Column("end_date")]
    public DateOnly EndDate { get; set; }

    [Column("status")]
    [StringLength(50)]
    [Unicode(false)]
    public string Status { get; set; } = null!;

    [Column("created_at", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at", TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("LeaveRequests")]
    public virtual User? User { get; set; }
}
