using System.Net.Mail;
using System.Text.RegularExpressions;
using TV5_VolunteerEventMgmtApp.Utilities;
using TV5_VolunteerEventMgmtApp.Utilities.Csv;

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

        public CsvValidationResponse IsValid()
        {
            var res = new CsvValidationResponse();


            // if first/last name is filled in they both must be filled in

            // city must not contain numbers  [a-zA-Z]

            if (!string.IsNullOrEmpty(DirectorFirstName))
            {
                if (!Regex.IsMatch(DirectorFirstName, @"^[a-zA-Z]+$"))
                {
                    res.IsValid = false;
                    res.Errors.Add("Please enter a valid last name for the director (a-z)");
                }
            }
            else
            {
                res.IsValid = false;
                res.Errors.Add("Please enter a first name for the director.");
            }

            if (!string.IsNullOrEmpty(DirectorLastName))
            {
                if (!Regex.IsMatch(DirectorLastName, @"^[a-zA-Z]+$"))
                {
                    res.IsValid = false;
                    res.Errors.Add("Please enter a last name for the director (a-z)");
                }
            }
            else
            {
                res.IsValid = false;
                res.Errors.Add("Please enter a last name for the director.");
            }

            if (!string.IsNullOrEmpty(DirectorEmail))
            {
                try
                {
                    var m = new MailAddress(DirectorEmail);
                }
                catch
                {
                    res.IsValid = false;
                    res.Errors.Add("The email provided is invalid.");
                }
            }

            if (!string.IsNullOrEmpty(DirectorPhone))
            {
                if (!ValidationUtilities.IsPhoneNumber(DirectorPhone))
                {
                    res.IsValid = false;
                    res.Errors.Add("Please enter a valid phone number.");
                }
            }

            return res;
        }

        public bool IsAddingNewDirector()
        {
                if (string.IsNullOrEmpty(DirectorEmail) && string.IsNullOrEmpty(DirectorPhone))
                {
                return false;
                }
            return true;
             // this isn't a solid check and only really works with my validation in the CSV Controller I wouldn't recommend using this
            // outside of that context

        }
    }
}
