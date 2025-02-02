﻿// <auto-generated />
using System;
using Backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Backend.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250202111556_SeedTestData")]
    partial class SeedTestData
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.1");

            modelBuilder.Entity("Backend.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("HireDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Employees");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DateOfBirth = new DateTime(1990, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "john.doe@example.com",
                            FirstName = "John",
                            HireDate = new DateTime(2025, 2, 2, 11, 15, 56, 181, DateTimeKind.Utc).AddTicks(9842),
                            LastName = "Doe",
                            PasswordHash = "VN5/YG8lI8uo76wXP6tC+39Z1Wzv+XTI/bc0LPLP40U=",
                            Role = "Employee"
                        });
                });

            modelBuilder.Entity("Backend.Models.Worktime", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("End")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Start")
                        .HasColumnType("TEXT");

                    b.Property<string>("Task")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.ToTable("Worktimes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            EmployeeId = 1,
                            End = new DateTime(2024, 2, 1, 17, 0, 0, 0, DateTimeKind.Unspecified),
                            Start = new DateTime(2024, 2, 1, 9, 0, 0, 0, DateTimeKind.Unspecified),
                            Task = "Worked on frontend UI"
                        },
                        new
                        {
                            Id = 2,
                            EmployeeId = 1,
                            End = new DateTime(2024, 2, 2, 16, 0, 0, 0, DateTimeKind.Unspecified),
                            Start = new DateTime(2024, 2, 2, 10, 0, 0, 0, DateTimeKind.Unspecified),
                            Task = "Bug fixes and testing"
                        });
                });

            modelBuilder.Entity("Backend.Models.Worktime", b =>
                {
                    b.HasOne("Backend.Models.Employee", "Employee")
                        .WithMany("Worktimes")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("Backend.Models.Employee", b =>
                {
                    b.Navigation("Worktimes");
                });
#pragma warning restore 612, 618
        }
    }
}
