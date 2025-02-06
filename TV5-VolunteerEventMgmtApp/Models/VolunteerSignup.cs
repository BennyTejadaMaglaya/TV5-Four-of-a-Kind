using Microsoft.Build.ObjectModelRemoting;
using System.ComponentModel.DataAnnotations;

namespace TV5_VolunteerEventMgmtApp.Models
{
	public class VolunteerSignup : IValidatableObject
	{
		
		public int Id { get; set; }

		[Display(Name ="Shift Start")]
		public DateTime StartTime { get; set; }

		[Display(Name ="Shift End")]
		public DateTime EndTime { get; set; }

		[Display(Name ="Time Slots")]
		public int TimeSlots { get; set; }

		[Display(Name ="Event")]
		public int VolunteerEventId { get; set; }
		public VolunteerEvent? VolunteerEvent { get; set; }

		[Display(Name ="Scheduled")]
		public ICollection<VolunteerAttendee> VolunteerAttendees { get; set; }


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

			if(StartTime < VolunteerEvent.StartTime && VolunteerEvent != null)
			{
				yield return new ValidationResult("A time slot must start either after or when an event is scheduled to start.", ["StartTime"]);
			}
			else if(StartTime > VolunteerEvent.EndTime && VolunteerEvent != null)
			{
				yield return new ValidationResult("A shift cannot start after the event has ended.", ["StartTime"]);
			}

			if (EndTime < VolunteerEvent.StartTime && VolunteerEvent != null)
			{
				yield return new ValidationResult("A time slot must end after an event is scheduled to start.", ["EndTime"]);
			}
			else if (EndTime > VolunteerEvent.EndTime && VolunteerEvent != null)
			{
				yield return new ValidationResult("A shift cannot end after the event has ended.", ["EndTime"]);
			}
			


		}
	}
}
