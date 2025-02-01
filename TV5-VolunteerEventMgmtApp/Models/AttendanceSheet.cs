using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TV5_VolunteerEventMgmtApp.Models
{
    public class AttendanceSheet : IValidatableObject
    {

        public int Id { get; set; }

		[Display(Name = "Director")]
		public int? DirectorId { get; set; }

		public Director? Director { get; set; }

		[Display(Name = "Attendance")]
		public ICollection<Attendee> Attendees { get; set; } = new HashSet<Attendee>();

        [StringLength(250)]
        public string? Notes { get; set; } = "";

        [Display(Name = "Start Time")]
        [Required(ErrorMessage = "Please enter a start time")]
        public DateTime StartTime { get; set; }

		[Display(Name = "End Time")]
        [Required(ErrorMessage ="Please enter a end time")]
        public DateTime EndTime { get; set; }


        // this is used to capture the number of singers at this moment of time.
        public int TotalSingers { get; set; }

		[Display(Name = "Location")]
		public int LocationId { get; set; }

        public Location? Location { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EndTime < StartTime)
            {
                yield return new ValidationResult("End Time cannot be earlier than the start Time.", ["EndTime"]);
            }
            else if (EndTime.Day != StartTime.Day)
            {
                yield return new ValidationResult("End Time cannont be a differnt day than Start Time.", ["EndTime"]);
            }

            if(StartTime > DateTime.Now.AddDays(1))
            {
                yield return new ValidationResult("Cannot create attendance sheets for the future." , ["StartTime"]);
            }
        }
	}
}
