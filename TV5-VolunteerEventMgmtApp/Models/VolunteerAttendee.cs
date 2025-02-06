using System.ComponentModel.DataAnnotations;

namespace TV5_VolunteerEventMgmtApp.Models
{
	public class VolunteerAttendee : IValidatableObject
	{

	
		public int VolunteerId { get; set;}
		public Volunteer? Volunteer { get; set;}

		public int VolunteerSignupId { get; set;}
		public VolunteerSignup? VolunteerSignup { get; set;}


		[Display(Name ="Arrival Time")]
		public DateTime? ArrivalTime { get; set; }

		[Display(Name ="Departure Time")]
		public DateTime? DepartureTime { get; set; }


		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if (DepartureTime < ArrivalTime)
			{
				yield return new ValidationResult("End Time cannot be earlier than the start Time.", ["DepartureTime"]);
			}
			else if (DepartureTime.Value.Date != ArrivalTime.Value.Date)
			{
				yield return new ValidationResult("End Time cannont be a differnt day than Start Time.", ["DepartureTime"]);
			}

			if(ArrivalTime > VolunteerSignup.StartTime.AddHours(1))
			{
				Volunteer.TimesLate++;
			}

			if(DepartureTime > VolunteerSignup.EndTime) {
				// maybe send a message saying the volunteer went late?
				// like maybe we need a text field to give the reason the volunteer went late.
				DepartureTime = VolunteerSignup.EndTime;
			}

		}
	}
}
