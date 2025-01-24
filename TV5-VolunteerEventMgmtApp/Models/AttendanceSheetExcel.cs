using TV5_VolunteerEventMgmtApp.Utilities;

namespace TV5_VolunteerEventMgmtApp.Models
{
    public class AttendanceSheetExcel
    {

        public string Location_Name { get; set; }
        public string Director_Name { get; set; }
        public int Available_Singers { get; set; }
        public int Total_Attendees { get; set; }
        public string Percentage_Attended { get; set; }
    }
}
