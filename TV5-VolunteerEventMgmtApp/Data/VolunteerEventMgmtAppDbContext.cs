using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using TV5_VolunteerEventMgmtApp.Models;

namespace TV5_VolunteerEventMgmtApp.Data
{
    public class VolunteerEventMgmtAppDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public string UserName
        {
            get; private set;
        }
        public VolunteerEventMgmtAppDbContext(DbContextOptions<VolunteerEventMgmtAppDbContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            if (_httpContextAccessor.HttpContext != null)
            {
                //We have a HttpContext, but there might not be anyone Authenticated
                UserName = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Unknown";
            }
            else
            {
                //No HttpContext so seeding data
                UserName = "Seed Data";
            }
        }
        public VolunteerEventMgmtAppDbContext(DbContextOptions<VolunteerEventMgmtAppDbContext> options)
            : base(options)
        {
            _httpContextAccessor = null!;
            UserName = "Seed Data";
        }

        public DbSet<Director> Directors { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<DirectorLocation> DirectorLocations { get; set; }
        public DbSet<Singer> Singers { get; set; }
        public DbSet<Attendee> Attendees { get; set; }
        public DbSet<AttendanceSheet> AttendeesSheets { get; set; }
        public DbSet<Venue> Venues { get; set; }
        public DbSet<SingerLocation> SingerLocations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //this disables the delete from a director to the Location.
            modelBuilder.Entity<Director>()
                .HasMany<DirectorLocation>(d => d.DirectorLocations)
                .WithOne(d => d.Director)
                .HasForeignKey(d => d.DirectorID)
                .OnDelete(DeleteBehavior.Restrict);

            //i believe this one will require cascade delete
            modelBuilder.Entity<Location>()
                .HasMany<AttendanceSheet>(d => d.AttendanceSheets)
                .WithOne(d => d.Location)
                .HasForeignKey(d => d.LocationId);

            modelBuilder.Entity<Location>()
                .HasMany<Venue>(d => d.Venues)
                .WithOne(d => d.Location)
                .HasForeignKey(d => d.LocationId);

            modelBuilder.Entity<Singer>()
                .HasMany<Attendee>(d => d.Attendance)
                .WithOne(d => d.Singer)
                .HasForeignKey(d => d.SingerId)
                .OnDelete(DeleteBehavior.Restrict);

            // this might need cascade delete?
            modelBuilder.Entity<AttendanceSheet>()
                .HasMany<Attendee>(d => d.Attendees)
                .WithOne(d => d.AttendanceSheet)
                .HasForeignKey(d => d.AttendenceSheetId)
                .OnDelete(DeleteBehavior.Restrict);

          
            modelBuilder.Entity<Singer>()
                .HasMany<SingerLocation>(d => d.SingerLocation)
                .WithOne(d => d.Singer)
                .HasForeignKey(d => d.SingerId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Location>()
                .HasMany<SingerLocation>(d => d.SingerLocations)
                .WithOne(d => d.Location)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.Restrict);

          



            //Many to Many intersections

            modelBuilder.Entity<Attendee>()
                .HasKey(d => new { d.SingerId, d.AttendenceSheetId });

            modelBuilder.Entity<DirectorLocation>()
                .HasKey(d => new { d.LocationID, d.DirectorID });
            modelBuilder.Entity<SingerLocation>()
                .HasKey(d => new { d.SingerId, d.LocationId });


        }
    }
}
