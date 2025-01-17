namespace TV5_VolunteerEventMgmtApp.Models
{
    public class Attendee
    {
        public int Id { get; set; }
        public Singer? Singer { get; set; }
        public int SingerId { get; set; }
        public int AttendenceSheetId { get; set; }
        public AttendanceSheet? AttendanceSheet { get; set; }
    }
}
