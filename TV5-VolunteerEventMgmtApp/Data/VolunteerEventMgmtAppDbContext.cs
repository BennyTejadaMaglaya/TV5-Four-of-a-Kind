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
        public DbSet<UploadedFile> UploadedFiles { get; set; }
        public DbSet<DirectorPhoto> DirectorPhotos { get; set; }
        public DbSet<DirectorThumbnail> DirectorThumbnails { get; set; }
        public DbSet<VolunteerSignup> VolunteerSignups { get; set; }
        public DbSet<VolunteerAttendee> VolunteerAttendees { get; set; }
        public DbSet<VolunteerLocation> VolunteerLocations { get; set; }
        public DbSet<VolunteerEvent> VolunteerEvents { get; set; }
        public DbSet<Volunteer> Volunteers { get; set; }
		public DbSet<HomeImage> HomeImages { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Location>()
                .HasMany<VolunteerEvent>(d => d.VolunteerEvents)
                .WithOne(d => d.Location)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.Restrict);
            

            //i believe this one will require cascade delete
            modelBuilder.Entity<Location>()
                .HasMany<AttendanceSheet>(d => d.AttendanceSheets)
                .WithOne(d => d.Location)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Location>()
                .HasMany<Venue>(d => d.Venues)
                .WithOne(d => d.Location)
                .HasForeignKey(d => d.LocationId);

            modelBuilder.Entity<VolunteerEvent>()
                .HasMany<VolunteerSignup>(d => d.TimeSlots)
                .WithOne(d => d.VolunteerEvent)
                .HasForeignKey(d => d.VolunteerEventId);

            // this might need cascade delete?
            modelBuilder.Entity<AttendanceSheet>()
                .HasMany<Attendee>(d => d.Attendees)
                .WithOne(d => d.AttendanceSheet)
                .HasForeignKey(d => d.AttendenceSheetId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Location>()
                .HasMany<SingerLocation>(d => d.SingerLocations)
                .WithOne(d => d.Location)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Location>()
                .HasMany<DirectorLocation>(d => d.DirectorLocations)
                .WithOne(d => d.Location)
                .HasForeignKey(d => d.LocationID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Location>()
                .HasMany<VolunteerLocation>(d => d.VolunteerLocations)
                .WithOne(d => d.Location)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<VolunteerSignup>()
                .HasMany(d => d.VolunteerAttendees)
                .WithOne(d => d.VolunteerSignup)
                .HasForeignKey(d => d.VolunteerSignupId) 
                .OnDelete(DeleteBehavior.Restrict);
          



            //Many to Many intersections

            modelBuilder.Entity<Attendee>()
                .HasKey(d => new { d.SingerId, d.AttendenceSheetId });

            modelBuilder.Entity<DirectorLocation>()
                .HasKey(d => new { d.LocationID, d.DirectorID });
            modelBuilder.Entity<SingerLocation>()
                .HasKey(d => new { d.SingerId, d.LocationId });
            modelBuilder.Entity<VolunteerLocation>()
                .HasKey(d => new { d.VolunteerId, d.LocationId });
            modelBuilder.Entity<VolunteerAttendee>()
                .HasKey(d => new { d.VolunteerSignupId, d.VolunteerId });



            modelBuilder.Entity<Singer>()
                .HasIndex(d => new { d.FirstName, d.LastName, d.DOB })
                .IsUnique();

            modelBuilder.Entity<Director>()
                .HasIndex(d => d.Email)
                .IsUnique();

            modelBuilder.Entity<Location>()
                .HasIndex(d => d.City)
                .IsUnique();
            modelBuilder.Entity<Volunteer>()
                .HasIndex(d => d.EmailAddress)
                .IsUnique();
            modelBuilder.Entity<VolunteerAttendee>()
                .HasIndex(d => new {d.VolunteerId, d.VolunteerSignupId})
                .IsUnique();
                


        }
    }
}
