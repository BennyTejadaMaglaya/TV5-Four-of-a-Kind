﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TV5_VolunteerEventMgmtApp.Data;

#nullable disable

namespace TV5_VolunteerEventMgmtApp.Data.VolunteerMigrations
{
    [DbContext(typeof(VolunteerEventMgmtAppDbContext))]
    [Migration("20250125223201_v0.011")]
    partial class v0011
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.11");

            modelBuilder.Entity("TV5_VolunteerEventMgmtApp.Models.AttendanceSheet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("DirectorId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("LocationId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("TotalSingers")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("DirectorId");

                    b.HasIndex("LocationId");

                    b.ToTable("AttendeesSheets");
                });

            modelBuilder.Entity("TV5_VolunteerEventMgmtApp.Models.Attendee", b =>
                {
                    b.Property<int>("SingerId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("AttendenceSheetId")
                        .HasColumnType("INTEGER");

                    b.HasKey("SingerId", "AttendenceSheetId");

                    b.HasIndex("AttendenceSheetId");

                    b.ToTable("Attendees");
                });

            modelBuilder.Entity("TV5_VolunteerEventMgmtApp.Models.Director", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Directors");
                });

            modelBuilder.Entity("TV5_VolunteerEventMgmtApp.Models.DirectorLocation", b =>
                {
                    b.Property<int>("LocationID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DirectorID")
                        .HasColumnType("INTEGER");

                    b.HasKey("LocationID", "DirectorID");

                    b.HasIndex("DirectorID");

                    b.ToTable("DirectorLocations");
                });

            modelBuilder.Entity("TV5_VolunteerEventMgmtApp.Models.Location", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(125)
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("City")
                        .IsUnique();

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("TV5_VolunteerEventMgmtApp.Models.Singer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateOnly>("DOB")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Phone")
                        .HasColumnType("TEXT");

                    b.Property<bool>("isActive")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("FirstName", "LastName", "DOB")
                        .IsUnique();

                    b.ToTable("Singers");
                });

            modelBuilder.Entity("TV5_VolunteerEventMgmtApp.Models.SingerLocation", b =>
                {
                    b.Property<int>("SingerId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("LocationId")
                        .HasColumnType("INTEGER");

                    b.HasKey("SingerId", "LocationId");

                    b.HasIndex("LocationId");

                    b.ToTable("SingerLocations");
                });

            modelBuilder.Entity("TV5_VolunteerEventMgmtApp.Models.Venue", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ContactEmail")
                        .HasColumnType("TEXT");

                    b.Property<string>("ContactName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("ContactPhone")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<int>("LocationId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("VenueName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("LocationId");

                    b.ToTable("Venues");
                });

            modelBuilder.Entity("TV5_VolunteerEventMgmtApp.Models.AttendanceSheet", b =>
                {
                    b.HasOne("TV5_VolunteerEventMgmtApp.Models.Director", "Director")
                        .WithMany("AttendanceSheets")
                        .HasForeignKey("DirectorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TV5_VolunteerEventMgmtApp.Models.Location", "Location")
                        .WithMany("AttendanceSheets")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Director");

                    b.Navigation("Location");
                });

            modelBuilder.Entity("TV5_VolunteerEventMgmtApp.Models.Attendee", b =>
                {
                    b.HasOne("TV5_VolunteerEventMgmtApp.Models.AttendanceSheet", "AttendanceSheet")
                        .WithMany("Attendees")
                        .HasForeignKey("AttendenceSheetId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("TV5_VolunteerEventMgmtApp.Models.Singer", "Singer")
                        .WithMany("Attendance")
                        .HasForeignKey("SingerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("AttendanceSheet");

                    b.Navigation("Singer");
                });

            modelBuilder.Entity("TV5_VolunteerEventMgmtApp.Models.DirectorLocation", b =>
                {
                    b.HasOne("TV5_VolunteerEventMgmtApp.Models.Director", "Director")
                        .WithMany("DirectorLocations")
                        .HasForeignKey("DirectorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TV5_VolunteerEventMgmtApp.Models.Location", "Location")
                        .WithMany("DirectorLocations")
                        .HasForeignKey("LocationID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Director");

                    b.Navigation("Location");
                });

            modelBuilder.Entity("TV5_VolunteerEventMgmtApp.Models.SingerLocation", b =>
                {
                    b.HasOne("TV5_VolunteerEventMgmtApp.Models.Location", "Location")
                        .WithMany("SingerLocations")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("TV5_VolunteerEventMgmtApp.Models.Singer", "Singer")
                        .WithMany("SingerLocation")
                        .HasForeignKey("SingerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Location");

                    b.Navigation("Singer");
                });

            modelBuilder.Entity("TV5_VolunteerEventMgmtApp.Models.Venue", b =>
                {
                    b.HasOne("TV5_VolunteerEventMgmtApp.Models.Location", "Location")
                        .WithMany("Venues")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Location");
                });

            modelBuilder.Entity("TV5_VolunteerEventMgmtApp.Models.AttendanceSheet", b =>
                {
                    b.Navigation("Attendees");
                });

            modelBuilder.Entity("TV5_VolunteerEventMgmtApp.Models.Director", b =>
                {
                    b.Navigation("AttendanceSheets");

                    b.Navigation("DirectorLocations");
                });

            modelBuilder.Entity("TV5_VolunteerEventMgmtApp.Models.Location", b =>
                {
                    b.Navigation("AttendanceSheets");

                    b.Navigation("DirectorLocations");

                    b.Navigation("SingerLocations");

                    b.Navigation("Venues");
                });

            modelBuilder.Entity("TV5_VolunteerEventMgmtApp.Models.Singer", b =>
                {
                    b.Navigation("Attendance");

                    b.Navigation("SingerLocation");
                });
#pragma warning restore 612, 618
        }
    }
}
