using TV5_VolunteerEventMgmtApp.Utilities;

namespace TV5_VolunteerEventMgmtApp.Models
{
    public class AttendanceSheetExcel
    {

        public string LocationName { get; set; }
        public string DirectorName { get; set; }
        public int AvailableSingers { get; set; }
        public int TotalAttendees { get; set; }
        public string PercentageAttended { get; set; }
    }
}
