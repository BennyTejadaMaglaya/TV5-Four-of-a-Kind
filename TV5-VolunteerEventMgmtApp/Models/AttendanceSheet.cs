using System.ComponentModel.DataAnnotations;

namespace TV5_VolunteerEventMgmtApp.Models
{
    public class AttendanceSheet
    {

        public int Id { get; set; }
        public int DirectorId { get; set; }
        public ICollection<Attendee> Attendees { get; set; } = new HashSet<Attendee>();//
        public string Notes { get; set; } = "";
        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }
        [Display(Name = "End Time")]
        public DateTime EndTime { get; set; }
    }
}
