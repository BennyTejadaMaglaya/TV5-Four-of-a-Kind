using TV5_VolunteerEventMgmtApp.Models;

namespace TV5_VolunteerEventMgmtApp.ViewModels
{
	public class VolunteerEventVM
	{
		public ICollection<VolunteerEvent> Events { get; set; }
		public VolunteerEvent newEvent { get; set; }

		public VolunteerSignup signup { get; set; }

		public VolunteerAttendee attendee { get; set; }

		public VolunteerLocation location { get; set; }

	}
}
