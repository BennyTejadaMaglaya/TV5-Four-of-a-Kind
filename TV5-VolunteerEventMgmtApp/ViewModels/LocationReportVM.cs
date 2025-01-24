namespace TV5_VolunteerEventMgmtApp.ViewModels
{
    public class LocationReportVM

    {
        public ICollection<LocationReportItem> Items { get; set; } = new List<LocationReportItem>();
        public AverageValue MinAvgAttendance { get; set; }
        public AverageValue MaxAvgAttendance { get; set; }
        public LocationReportVM(ICollection<LocationReportItem> items) 
        { 
            Items = items;

            if(Items.Any())
            {
                var low = Items.OrderBy(s => s.Average_Attendees).FirstOrDefault();
                var high = Items.OrderByDescending(s => s.Average_Attendees).FirstOrDefault();
                MinAvgAttendance = new AverageValue { Average= low.Average_Attendees, Name=low.City };
               
                MaxAvgAttendance = new AverageValue { Average = high.Average_Attendees, Name = high.City };
            } // need to extract name
        }
    }

    public struct AverageValue()
    {
        public double Average { get; set; }
        public string Name { get; set; }
    }
}
