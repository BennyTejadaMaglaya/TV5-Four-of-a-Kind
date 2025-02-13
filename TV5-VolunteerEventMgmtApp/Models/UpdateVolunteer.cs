using System.ComponentModel.DataAnnotations;

namespace TV5_VolunteerEventMgmtApp.Models
{
    public class UpdateVolunteer
    {
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(125, ErrorMessage = "A first name must be between 2 and 125 characters.", MinimumLength = 2)]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(125, ErrorMessage = "A first name must be between 2 and 125 characters.", MinimumLength = 2)]
        public string LastName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage ="An email address is required")]

        [EmailAddress(ErrorMessage = "please enter a valid Email adress")]
        public string Email { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage ="A phone number is required.")]
        [Phone(ErrorMessage = "plase enter a valid phone number")]
        [Length(10, 10, ErrorMessage = "A phone number is 10 digits long.")]
        public string PhoneNumber { get; set; }
    }
}
