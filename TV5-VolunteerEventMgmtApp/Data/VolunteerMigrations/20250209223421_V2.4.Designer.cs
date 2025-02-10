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
    [Migration("20250209223421_V2.4")]
    partial class V24
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

                    b.Property<int?>("DirectorId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("LocationId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Notes")
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
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(10)
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

            modelBuilder.Entity("TV5_VolunteerEventMgmtApp.Models.DirectorPhoto", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("Content")
                        .HasColumnType("BLOB");

                    b.Property<int>("DirectorId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("MimeType")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("DirectorId");

                    b.ToTable("DirectorPhotos");
                });

            modelBuilder.Entity("TV5_VolunteerEventMgmtApp.Models.DirectorThumbnail", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("Content")
                        .HasColumnType("BLOB");

                    b.Property<int>("DirectorID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("MimeType")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("DirectorID");

                    b.ToTable("DirectorThumbnails");
                });

            modelBuilder.Entity("TV5_VolunteerEventMgmtApp.Models.FileContent", b =>
                {
                    b.Property<int>("FileContentID")
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("Content")
                        .HasColumnType("BLOB");

                    b.HasKey("FileContentID");

                    b.ToTable("FileContent");
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

                    b.Property<string>("Color")
                        .IsRequired()
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

                    b.Property<string>("EmergencyContactName")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("Notes")
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.Property<string>("Phone")
                        .HasMaxLength(10)
                        .HasColumnType("TEXT");

                    b.Property<int?>("Relation")
                        .HasColumnType("INTEGER");

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

            modelBuilder.Entity("TV5_VolunteerEventMgmtApp.Models.UploadedFile", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("FileName")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("MimeType")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("UploadedFiles");
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

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<int>("LocationId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("NumPastEvents")
                        .HasColumnType("INTEGER");

                    b.Property<string>("VenueName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("LocationId");

                    b.ToTable("Venues");
                });

            modelBuilder.Entity("TV5_VolunteerEventMgmtApp.Models.Volunteer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(125)
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("JoinDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(125)
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("TimesLate")
                        .HasColumnType("INTEGER");

                    b.Property<int>("numShifts")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("EmailAddress")
                        .IsUnique();

                    b.ToTable("Volunteers");
                });

            modelBuilder.Entity("TV5_VolunteerEventMgmtApp.Models.VolunteerAttendee", b =>
                {
                    b.Property<int>("VolunteerSignupId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("VolunteerId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("ArrivalTime")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DepartureTime")
                        .HasColumnType("TEXT");

                    b.HasKey("VolunteerSignupId", "VolunteerId");

                    b.HasIndex("VolunteerId", "VolunteerSignupId")
                        .IsUnique();

                    b.ToTable("VolunteerAttendees");
                });

            modelBuilder.Entity("TV5_VolunteerEventMgmtApp.Models.VolunteerEvent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<int>("LocationId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(125)
                        .HasColumnType("TEXT");

                    b.Property<int>("VenueId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.HasIndex("VenueId");

                    b.ToTable("VolunteerEvents");
                });

            modelBuilder.Entity("TV5_VolunteerEventMgmtApp.Models.VolunteerLocation", b =>
                {
                    b.Property<int>("VolunteerId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("LocationId")
                        .HasColumnType("INTEGER");

                    b.HasKey("VolunteerId", "LocationId");

                    b.HasIndex("LocationId");

                    b.ToTable("VolunteerLocations");
                });

            modelBuilder.Entity("TV5_VolunteerEventMgmtApp.Models.VolunteerSignup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("TimeSlots")
                        .HasColumnType("INTEGER");

                    b.Property<int>("VolunteerEventId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("VolunteerEventId");

                    b.ToTable("VolunteerSignups");
                });

            modelBuilder.Entity("TV5_VolunteerEventMgmtApp.Models.AttendanceSheet", b =>
                {
                    b.HasOne("TV5_VolunteerEventMgmtApp.Models.Director", "Director")
                        .WithMany("AttendanceSheets")
                        .HasForeignKey("DirectorId");

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
                        .OnDelete(DeleteBehavior.Cascade)
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
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Director");

                    b.Navigation("Location");
                });

            modelBuilder.Entity("TV5_VolunteerEventMgmtApp.Models.DirectorPhoto", b =>
                {
                    b.HasOne("TV5_VolunteerEventMgmtApp.Models.Director", "Director")
                        .WithMany()
                        .HasForeignKey("DirectorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Director");
                });

            modelBuilder.Entity("TV5_VolunteerEventMgmtApp.Models.DirectorThumbnail", b =>
                {
                    b.HasOne("TV5_VolunteerEventMgmtApp.Models.Director", "Director")
                        .WithMany()
                        .HasForeignKey("DirectorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Director");
                });

            modelBuilder.Entity("TV5_VolunteerEventMgmtApp.Models.FileContent", b =>
                {
                    b.HasOne("TV5_VolunteerEventMgmtApp.Models.UploadedFile", "UploadedFile")
                        .WithOne("FileContent")
                        .HasForeignKey("TV5_VolunteerEventMgmtApp.Models.FileContent", "FileContentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UploadedFile");
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

            modelBuilder.Entity("TV5_VolunteerEventMgmtApp.Models.VolunteerAttendee", b =>
                {
                    b.HasOne("TV5_VolunteerEventMgmtApp.Models.Volunteer", "Volunteer")
                        .WithMany()
                        .HasForeignKey("VolunteerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TV5_VolunteerEventMgmtApp.Models.VolunteerSignup", "VolunteerSignup")
                        .WithMany("VolunteerAttendees")
                        .HasForeignKey("VolunteerSignupId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Volunteer");

                    b.Navigation("VolunteerSignup");
                });

            modelBuilder.Entity("TV5_VolunteerEventMgmtApp.Models.VolunteerEvent", b =>
                {
                    b.HasOne("TV5_VolunteerEventMgmtApp.Models.Location", "Location")
                        .WithMany("VolunteerEvents")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("TV5_VolunteerEventMgmtApp.Models.Venue", "Venue")
                        .WithMany("EventVenues")
                        .HasForeignKey("VenueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Location");

                    b.Navigation("Venue");
                });

            modelBuilder.Entity("TV5_VolunteerEventMgmtApp.Models.VolunteerLocation", b =>
                {
                    b.HasOne("TV5_VolunteerEventMgmtApp.Models.Location", "Location")
                        .WithMany("VolunteerLocations")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("TV5_VolunteerEventMgmtApp.Models.Volunteer", "Volunteer")
                        .WithMany("VolunteerLocations")
                        .HasForeignKey("VolunteerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Location");

                    b.Navigation("Volunteer");
                });

            modelBuilder.Entity("TV5_VolunteerEventMgmtApp.Models.VolunteerSignup", b =>
                {
                    b.HasOne("TV5_VolunteerEventMgmtApp.Models.VolunteerEvent", "VolunteerEvent")
                        .WithMany("TimeSlots")
                        .HasForeignKey("VolunteerEventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("VolunteerEvent");
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

                    b.Navigation("VolunteerEvents");

                    b.Navigation("VolunteerLocations");
                });

            modelBuilder.Entity("TV5_VolunteerEventMgmtApp.Models.Singer", b =>
                {
                    b.Navigation("Attendance");

                    b.Navigation("SingerLocation");
                });

            modelBuilder.Entity("TV5_VolunteerEventMgmtApp.Models.UploadedFile", b =>
                {
                    b.Navigation("FileContent");
                });

            modelBuilder.Entity("TV5_VolunteerEventMgmtApp.Models.Venue", b =>
                {
                    b.Navigation("EventVenues");
                });

            modelBuilder.Entity("TV5_VolunteerEventMgmtApp.Models.Volunteer", b =>
                {
                    b.Navigation("VolunteerLocations");
                });

            modelBuilder.Entity("TV5_VolunteerEventMgmtApp.Models.VolunteerEvent", b =>
                {
                    b.Navigation("TimeSlots");
                });

            modelBuilder.Entity("TV5_VolunteerEventMgmtApp.Models.VolunteerSignup", b =>
                {
                    b.Navigation("VolunteerAttendees");
                });
#pragma warning restore 612, 618
        }
    }
}
