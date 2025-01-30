namespace TV5_VolunteerEventMgmtApp.Models
{
    public class DirectorCsvUpload
    {
        public string FirstName {  get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string LocationName { get; set; } // IM MAKING IT SO THE LOCATION MUST EXIST WHEN IMPORTING DIRECTORS
        // my reasoning for this is if the user misspells a nme then they hve an extra location and have to fix it.
        // location import will allow for the user to create a director with it though

    }
}
