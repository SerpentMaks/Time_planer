using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Time_planer.Models;

namespace Time_planer.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<LeaveRequest> LeaveRequests { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<WorkTimeEntry> WorkTimeEntries { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-FBT8LJ4\\SQLEXPRESS;Database=TimeManagementDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LeaveRequest>(entity =>
        {
            entity.HasKey(e => e.LeaveId).HasName("PK__leave_re__743350BC110FE00A");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.User).WithMany(p => p.LeaveRequests).HasConstraintName("FK_leave_requests_user");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__roles__760965CC4482023A");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__users__B9BE370FC4076626");

            entity.HasOne(d => d.Role).WithMany(p => p.Users).HasConstraintName("FK_users_roles");
        });

        modelBuilder.Entity<WorkTimeEntry>(entity =>
        {
            entity.HasKey(e => e.EntryId).HasName("PK__work_tim__810FDCE19101C9D4");

            entity.HasOne(d => d.Registrar).WithMany(p => p.WorkTimeEntryRegistrars).HasConstraintName("FK_work_time_entries_registrar");

            entity.HasOne(d => d.User).WithMany(p => p.WorkTimeEntryUsers).HasConstraintName("FK_work_time_entries_user");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
