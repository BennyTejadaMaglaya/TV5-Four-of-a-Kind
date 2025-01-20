using TV5_VolunteerEventMgmtApp.Models;

namespace TV5_VolunteerEventMgmtApp.ViewModels
{
    public class AttendanceSheetSummaryVM
    {
        public int TotalAttendees { get; set; }
        public AttendanceSheet Sheet { get; set; }
    }
}
