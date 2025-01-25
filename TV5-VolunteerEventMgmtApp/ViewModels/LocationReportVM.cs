namespace TV5_VolunteerEventMgmtApp.ViewModels
{
    public class LocationReportVM

    {
        public ICollection<LocationReportItem> Items { get; set; } = new List<LocationReportItem>();
        public NamedValue MinAvgAttendance { get; set; }
        public NamedValue MaxAvgAttendance { get; set; }
        public NamedValue MaxTotalSingers { get; set; }
        public NamedValue MinTotalSingers { get; set; }
        public int ActiveSingers { get; set; }
        
    }

    public struct NamedValue()
    {
        public double Value { get; set; }
        public string Name { get; set; }
    }
}
