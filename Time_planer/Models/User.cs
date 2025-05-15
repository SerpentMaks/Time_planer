using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Time_planer.Models;

[Table("users")]
[Index("Email", Name = "UQ__users__AB6E6164161C2231", IsUnique = true)]
public partial class User
{
    [Key]
    [Column("user_id")]
    public int UserId { get; set; }

    [Column("first_name")]
    [StringLength(100)]
    [Unicode(false)]
    public string FirstName { get; set; } = null!;

    [Column("last_name")]
    [StringLength(100)]
    [Unicode(false)]
    public string LastName { get; set; } = null!;

    [Column("email")]
    [StringLength(255)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [Column("password")]
    [StringLength(255)]
    [Unicode(false)]
    public string Password { get; set; } = null!;

    [Column("role_id")]
    public int? RoleId { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();

    [ForeignKey("RoleId")]
    [InverseProperty("Users")]
    public virtual Role? Role { get; set; }

    [InverseProperty("Registrar")]
    public virtual ICollection<WorkTimeEntry> WorkTimeEntryRegistrars { get; set; } = new List<WorkTimeEntry>();

    [InverseProperty("User")]
    public virtual ICollection<WorkTimeEntry> WorkTimeEntryUsers { get; set; } = new List<WorkTimeEntry>();
}
