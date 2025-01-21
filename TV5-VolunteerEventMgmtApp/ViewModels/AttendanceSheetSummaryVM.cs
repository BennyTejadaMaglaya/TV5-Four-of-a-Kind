using System.ComponentModel.DataAnnotations;

namespace TV5_VolunteerEventMgmtApp.ViewModels
{
    public class AttendanceSheetSummaryVm
    {
        [Display(Name ="Start Range")]
        public DateTime? StartDate { get; set; }
        [Display(Name = "End Range")]
        public DateTime? EndDate { get; set; }
        public ICollection<SheetSummaryItemVM> Items { get; set; } = new List<SheetSummaryItemVM>();
    }
}
