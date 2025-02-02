using TV5_VolunteerEventMgmtApp.Models;

namespace TV5_VolunteerEventMgmtApp.ViewModels
{
    public class DashboardVM
    {
        public List<Location> Locations { get; set; }

        public AttendanceSheet attendanceSheet { get; set; }
        public Singer newSinger { get; set; }
       
    }
}
