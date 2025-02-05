using System.ComponentModel.DataAnnotations;

namespace TV5_VolunteerEventMgmtApp.Models
{
	public class VolunteerEvent : IValidatableObject
	{
		public int Id { get; set; }

		[Display(Name ="Title")]
		[Required(ErrorMessage ="Title is required.")]
		[StringLength(125, ErrorMessage = "Event title must be less than 125 characters.")]
		public string Title { get; set; }

		// this can be used for admins to create events ahead of time so they can be published at a later date.
		// maybe in the view we just ask if they want to public the event immediately if not then ask to set a publish time?
		[Display(Name ="Published")]
		public bool IsActive { get; set; }	

		[Display(Name = "Start Time")]
		[Required(ErrorMessage = "Please enter a start time")]
		public DateTime StartTime { get; set; }

		[Display(Name = "End Time")]
		[Required(ErrorMessage = "Please enter a end time")]
		public DateTime EndTime { get; set; }

		[Display(Name ="Event description")]
		[Required(ErrorMessage ="An event requires a description.")]
		[StringLength(255, ErrorMessage ="Description must be less that 255 characters long.")]
		public string Description { get; set; }

		[Display(Name ="Time Slots")]
		public ICollection<VolunteerTime> TimeSlot { get; set; } = new HashSet<VolunteerTime>();

		[Display(Name ="Location")]
		public int LocationId { get; set; }
		public Location? Location { get; set; }

		[Display(Name ="Venue")]
		public int VenueId { get; set; }
		public Venue? Venue { get; set; }


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

			if (StartTime > DateTime.Now.AddDays(1))
			{
				yield return new ValidationResult("Cannot create attendance sheets for the future.", ["StartTime"]);
			}
		}
	}
}
