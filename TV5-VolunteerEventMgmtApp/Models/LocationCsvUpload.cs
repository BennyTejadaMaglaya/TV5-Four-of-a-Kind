namespace TV5_VolunteerEventMgmtApp.Models
{
    public class LocationCsvUpload
    { // we will check if the director with first/last name already exists
        // if only a first/last name are passed we check if they're in the database already 
        // ? what to do if multiple first/last names
        public string City { get; set; } 
        public string DirectorFirstName { get; set; } 
        public string DirectorLastName { get; set; }
        public string DirectorEmail { get; set; }
        public string DirectorPhone { get; set; }

    }
}
