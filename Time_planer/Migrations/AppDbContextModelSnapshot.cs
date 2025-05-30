﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Time_planer.Data;

#nullable disable

namespace Time_planer.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Time_planer.Models.LeaveRequest", b =>
                {
                    b.Property<int>("LeaveId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("leave_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LeaveId"));

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<DateOnly>("EndDate")
                        .HasColumnType("date")
                        .HasColumnName("end_date");

                    b.Property<string>("LeaveType")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("leave_type");

                    b.Property<DateOnly>("StartDate")
                        .HasColumnType("date")
                        .HasColumnName("start_date");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("status");

                    b.Property<DateTime?>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("updated_at")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<int?>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.HasKey("LeaveId")
                        .HasName("PK__leave_re__743350BC110FE00A");

                    b.HasIndex("UserId");

                    b.ToTable("leave_requests");
                });

            modelBuilder.Entity("Time_planer.Models.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("role_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoleId"));

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("role_name");

                    b.HasKey("RoleId")
                        .HasName("PK__roles__760965CC4482023A");

                    b.ToTable("roles");
                });

            modelBuilder.Entity("Time_planer.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("last_name");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("password");

                    b.Property<int?>("RoleId")
                        .HasColumnType("int")
                        .HasColumnName("role_id");

                    b.HasKey("UserId")
                        .HasName("PK__users__B9BE370FC4076626");

                    b.HasIndex("RoleId");

                    b.HasIndex(new[] { "Email" }, "UQ__users__AB6E6164161C2231")
                        .IsUnique();

                    b.ToTable("users");
                });

            modelBuilder.Entity("Time_planer.Models.WorkTimeEntry", b =>
                {
                    b.Property<int>("EntryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("entry_id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EntryId"));

                    b.Property<DateTime?>("EndTime")
                        .HasColumnType("datetime")
                        .HasColumnName("end_time");

                    b.Property<int?>("RegistrarId")
                        .HasColumnType("int")
                        .HasColumnName("registrar_id");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime")
                        .HasColumnName("start_time");

                    b.Property<decimal?>("TotalHours")
                        .HasColumnType("decimal(10, 2)")
                        .HasColumnName("total_hours");

                    b.Property<int?>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.HasKey("EntryId")
                        .HasName("PK__work_tim__810FDCE19101C9D4");

                    b.HasIndex("RegistrarId");

                    b.HasIndex("UserId");

                    b.ToTable("work_time_entries");
                });

            modelBuilder.Entity("Time_planer.Models.LeaveRequest", b =>
                {
                    b.HasOne("Time_planer.Models.User", "User")
                        .WithMany("LeaveRequests")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_leave_requests_user");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Time_planer.Models.User", b =>
                {
                    b.HasOne("Time_planer.Models.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .HasConstraintName("FK_users_roles");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Time_planer.Models.WorkTimeEntry", b =>
                {
                    b.HasOne("Time_planer.Models.User", "Registrar")
                        .WithMany("WorkTimeEntryRegistrars")
                        .HasForeignKey("RegistrarId")
                        .HasConstraintName("FK_work_time_entries_registrar");

                    b.HasOne("Time_planer.Models.User", "User")
                        .WithMany("WorkTimeEntryUsers")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_work_time_entries_user");

                    b.Navigation("Registrar");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Time_planer.Models.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("Time_planer.Models.User", b =>
                {
                    b.Navigation("LeaveRequests");

                    b.Navigation("WorkTimeEntryRegistrars");

                    b.Navigation("WorkTimeEntryUsers");
                });
#pragma warning restore 612, 618
        }
    }
}
