using TV5_VolunteerEventMgmtApp.Models;

namespace TV5_VolunteerEventMgmtApp.ViewModels
{
    public class AttendanceSheetVM
    {
        public int TotalAttendees { get; set; }
        public AttendanceSheet Sheet { get; set; }
        
        public PercentageColorVM Percentage { get; set; }

        public LocationReportVM LocationReport { get; set; }
    }
}
