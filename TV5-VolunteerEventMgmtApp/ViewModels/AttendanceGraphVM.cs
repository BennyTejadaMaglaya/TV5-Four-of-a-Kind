using TV5_VolunteerEventMgmtApp.Models;
using TV5_VolunteerEventMgmtApp.Utilities;

namespace TV5_VolunteerEventMgmtApp.ViewModels
{
    public class AttendanceGraphVM
    {
        public int LocationId { get; set; } = 1;
        
        public int? Year { get; set; }
        public int TotalRegistered { get; set; }
        public List<MonthWeekCellVM> heatMapCells { get; set; } = new List<MonthWeekCellVM>();
        
    }
}
