namespace TV5_VolunteerEventMgmtApp.ViewModels
{
    public class CsvUploadVM<T>
    {
        public T Data { get; set; }
        public int SuccessCount { get; set; } = 0;
    }
}
