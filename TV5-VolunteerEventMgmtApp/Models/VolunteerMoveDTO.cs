namespace TV5_VolunteerEventMgmtApp.Models
{
	public class VolunteerMoveDTO
	{
		public int volunteerId { get; set; }
		public int oldTimeslotId { get; set; }
		public int newTimeslotId { get; set; }
	}
}
