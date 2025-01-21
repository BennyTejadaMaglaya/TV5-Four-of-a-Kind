using TV5_VolunteerEventMgmtApp.Models;
using TV5_VolunteerEventMgmtApp.Utilities;

namespace TV5_VolunteerEventMgmtApp.ViewModels
{
    public class SheetSummaryItemVM
    {
        public int TotalAttendees { get; set; }
        public AttendanceSheet Sheet { get; set; }
        
        public PercentageColorVM Percentage { get; set; }
    }
}
