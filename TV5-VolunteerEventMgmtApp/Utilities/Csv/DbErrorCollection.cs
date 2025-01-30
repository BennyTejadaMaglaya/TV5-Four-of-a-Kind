namespace TV5_VolunteerEventMgmtApp.Utilities.Csv
{
    public class DbErrorCollection
    {
        public List<string> Errors { get; set; } = new List<string>();
        public int SuccessCount { get; set; } = 0;
    }
}
