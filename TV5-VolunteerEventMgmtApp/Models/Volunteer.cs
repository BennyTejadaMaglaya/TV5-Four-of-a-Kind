using System.ComponentModel.DataAnnotations;

namespace TV5_VolunteerEventMgmtApp.Models
{
	public class Volunteer
	{
		public int Id { get; set; }

		[Display(Name ="First Name")]
		[Required(ErrorMessage ="First name is required.")]
		[StringLength(125, ErrorMessage ="A first name must be between 2 and 125 characters.", MinimumLength = 2)]
		public string FirstName { get; set; }

		[Display(Name ="Last Name")]
		[Required(ErrorMessage ="Last name is required.")]
		[StringLength(125, ErrorMessage = "A first name must be between 2 and 125 characters.", MinimumLength = 2)]
		public string LastName { get; set; }

		[Display(Name ="Phone number")]
		
		[Phone(ErrorMessage ="plase enter a valid phone number")]
		[Length(10,10,ErrorMessage ="A phone number is 10 digits long.")]
		public string PhoneNumber { get; set; }

		[Display(Name ="Email Address")]
		
		[EmailAddress(ErrorMessage ="please enter a valid Email adress")]
		public string EmailAddress { get; set; }


		// maybe a little extra when we get to doing notifications to thank long tenured volunteers.
		[Display(Name ="Join Date")]
		public DateTime JoinDate { get; set; } = DateTime.Now;

		[Display(Name = "Late Shifts")]
		public int TimesLate { get; set; } = 0;

		[Display(Name = "Shifts Completed")]
		public int numShifts { get; set; } = 0;

		//used for the the soft delete
		public bool IsActive { get; set; }


		// used to allow admin to confirm a user before they are added to the system
		// this will allow them to be seen in the system but wont allow for the volunteer to sign up for events.
		public bool IsConfirmed { get; set; }

	}
}
