using Microsoft.CodeAnalysis.FlowAnalysis;
using System.Net.Mail;
using TV5_VolunteerEventMgmtApp.Utilities;
using TV5_VolunteerEventMgmtApp.Utilities.Csv;

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


        public CsvValidationResponse IsValid()
        {

            var response = new CsvValidationResponse();

            if(string.IsNullOrEmpty(FirstName))
            {
                response.Errors.Add("A first name is required when submitting a director. ");
                response.IsValid = false;
            }
            if (string.IsNullOrEmpty(LastName))
            {
                response.Errors.Add("A last name is required when submitting a director. ");
                response.IsValid = false;
            }
            if (string.IsNullOrEmpty(Email))
            {
                response.Errors.Add("An email is required when submitting a director.");
                response.IsValid = false;
            }
            if (!string.IsNullOrEmpty(PhoneNumber))
            {
                if (!ValidationUtilities.IsPhoneNumber(PhoneNumber))
                {
                    response.Errors.Add($"The phone number for singer {FirstName} {LastName} is invalid.");
                    response.IsValid = false;
                }
            }
            else
            {
                response.Errors.Add("You must submit a phone number for directors");
                response.IsValid=false;
            }
        
            if (string.IsNullOrEmpty(LocationName))
            {
                response.Errors.Add("You must submit an EXISTING location name (not case sensitive) for this director.");
                response.IsValid = false;
            }

            try
            {
                var m = new MailAddress(Email);
            }
            catch
            {
                response.Errors.Add("You must submit a valid Email address when uploading a director");
                response.IsValid = false;
            }


            return response;


        }

    }
}
