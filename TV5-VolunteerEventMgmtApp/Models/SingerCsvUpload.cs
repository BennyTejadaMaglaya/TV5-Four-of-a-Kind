using Azure;
using System.Net.Mail;
using TV5_VolunteerEventMgmtApp.Utilities;
using TV5_VolunteerEventMgmtApp.Utilities.Csv;

namespace TV5_VolunteerEventMgmtApp.Models
{
    public class SingerCsvUpload
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; } //not req
        public string PhoneNumber { get; set; }//not req
        public string DOB { get; set; }

        public string ContactName { get; set; }//not req

        public DateOnly DateOfBirth()
        {
            var res = DateOnly.TryParse(DOB, out var d);
           
            return d;
        }

        public bool DobIsValid()
        {
            var res = DateOnly.TryParse(DOB, out var _);
            return res;
        }

        public bool SingerMeetsAgeRequirement()
        {
            if(DateOnly.TryParse(DOB,out var dob))
            {
                if(DateTime.Now.Year - dob.Year >= 18)
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        public CsvValidationResponse IsValid()
        {
            var res = new CsvValidationResponse() { Errors = new List<String>(), IsValid=true };

            if (string.IsNullOrEmpty(FirstName))
            {
                res.Errors.Add($"You've entered a singer without a first name");
                res.IsValid = false;
            }

            if (string.IsNullOrEmpty(LastName))
            {
                res.Errors.Add($"You've entered a singer without a last name");
                res.IsValid = false;
            }
            if (!DobIsValid())
            {
                res.Errors.Add($"DOB for singer {FirstName} {LastName} is invalid (use format 08/19/2012)");
                res.IsValid = false;
            }
            if (!String.IsNullOrEmpty(PhoneNumber))
            {
                if (!ValidationUtilities.IsPhoneNumber(PhoneNumber))
                {
                    res.Errors.Add($"The phone number for singer {FirstName} {LastName} is invalid.");
                    res.IsValid = false;
                }
            }

            if (!string.IsNullOrEmpty(Email))
            {
                try
                {
                    var m = new MailAddress(Email);
                }
                catch
                {
                    res.Errors.Add("The email provided is invalid");
                    res.IsValid = false;
                }
            }
            
           

            if (!SingerMeetsAgeRequirement())
            {
                res.Errors.Add($"The singer {FirstName} {LastName} 18 or older.");
                res.IsValid = false;
            }

            

            return res;
        }
    }
}
