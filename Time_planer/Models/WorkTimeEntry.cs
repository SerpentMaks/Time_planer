using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Time_planer.Models;

[Table("work_time_entries")]
public partial class WorkTimeEntry
{
    [Key]
    [Column("entry_id")]
    public int EntryId { get; set; }

    [Column("user_id")]
    public int? UserId { get; set; }

    [Column("registrar_id")]
    public int? RegistrarId { get; set; }

    [Column("start_time", TypeName = "datetime")]
    public DateTime StartTime { get; set; }

    [Column("end_time", TypeName = "datetime")]
    public DateTime? EndTime { get; set; }

    [Column("total_hours", TypeName = "decimal(10, 2)")]
    public decimal? TotalHours { get; set; }

    [ForeignKey("RegistrarId")]
    [InverseProperty("WorkTimeEntryRegistrars")]
    public virtual User? Registrar { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("WorkTimeEntryUsers")]
    public virtual User? User { get; set; }
}
