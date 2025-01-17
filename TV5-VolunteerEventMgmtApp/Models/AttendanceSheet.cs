namespace TV5_VolunteerEventMgmtApp.Models
{
    public class AttendanceSheet
    {

        public int Id { get; set; }
        public int DirectorId { get; set; }
        public ICollection<Attendee> Attendees { get; set; } = new HashSet<Attendee>();
        public string Notes { get; set; } = "";
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
