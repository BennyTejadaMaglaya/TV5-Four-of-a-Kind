namespace TV5_VolunteerEventMgmtApp.Models
{
    public class SingerCsvUpload
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string DOB { get; set; }

        public DateOnly DateOfBirth()
        {
            var res = DateOnly.TryParse(DOB, out var d);
            return d;
        }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(FirstName) && !string.IsNullOrWhiteSpace(LastName) && !string.IsNullOrWhiteSpace(DOB);
        }
    }
}
