using System.ComponentModel.DataAnnotations;

namespace TV5_VolunteerEventMgmtApp.Models
{
    public class AttendanceSheet
    {

        public int Id { get; set; }

		[Display(Name = "Director")]
		public int DirectorId { get; set; }

		public Director? Director { get; set; }

		[Display(Name = "Singers")]
		public ICollection<Attendee> Attendees { get; set; } = new HashSet<Attendee>();

        [StringLength(250)]
        public string Notes { get; set; } = "";

        [Display(Name = "Start Time")]
        [Required(ErrorMessage = "Please enter a start time")]
        public DateTime StartTime { get; set; }

		[Display(Name = "End Time")]
        [Required(ErrorMessage ="Please enter a end time")]
        public DateTime EndTime { get; set; }

		[Display(Name = "Location")]
		public int LocationId { get; set; }

        public Location? Location { get; set; }
	}
}
