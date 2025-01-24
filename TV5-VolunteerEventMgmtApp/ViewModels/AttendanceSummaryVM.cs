namespace TV5_VolunteerEventMgmtApp.ViewModels
{
    public class AttendanceSummaryVM
    {
        public ICollection<AttendanceSheetVM> AttendanceSummaries { get; set; } = new List<AttendanceSheetVM>();
        public LocationReportVM LocationReport { get; set; }
    }
}
