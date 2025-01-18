using System.ComponentModel.DataAnnotations;
namespace TV5_VolunteerEventMgmtApp.Models
{
    public class Location
    {
        public int ID { get; set; }

        [Display(Name = "City")]
        [Required(ErrorMessage ="You cannot create a location without a city.")]
        [MaxLength(125, ErrorMessage = "City name cannot exeed 125 characters.")]
        public string City { get; set; }

        public bool IsActive { get; set; }

        public ICollection<DirectorLocation> Directors { get; set; } = new HashSet<DirectorLocation>();

        // public ICollection<VolunteerEvent> VolunteerEvents { get; set; }

        public ICollection<AttendanceSheet> AttendanceSheets { get; set; } = new HashSet<AttendanceSheet>();

        public ICollection<Venue> Venues { get; set; } = new HashSet<Venue>();

    }
}
