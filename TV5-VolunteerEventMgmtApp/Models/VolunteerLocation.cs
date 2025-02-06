namespace TV5_VolunteerEventMgmtApp.Models
{
	public class VolunteerLocation
	{

		public int LocationId { get; set; }
		public Location? Location { get; set; }

		public int VolunteerId { get; set;}
		public Volunteer? Volunteer { get; set;}
	}
}
