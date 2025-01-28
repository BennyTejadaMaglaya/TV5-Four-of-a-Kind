using System.ComponentModel.DataAnnotations;

namespace TV5_VolunteerEventMgmtApp.Models
{
    public class Director : IValidatableObject
    {
        public int ID { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "A first name is required when adding a new choir director.")]
        [StringLength(64, ErrorMessage = "A first name must be less than 64 characters long.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "A Last Name is required when creating a director.")]
        [StringLength(128, ErrorMessage ="A last name must be less than 128 characters long.")]
        public string LastName { get; set; }

        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "A Phone number is required when creating a director.")]
        [Phone(ErrorMessage = "Please enter a valid phone number")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Please enter a valid phone number")]
        [StringLength(10, ErrorMessage = "A phone number must be 10 numbers long.", MinimumLength = 10)]
        public string PhoneNumber { get; set; }

        [Display( Name = "Email Address")]
        [EmailAddress(ErrorMessage = "Please enter a valid EmailAddress")]
        [Required(ErrorMessage = "An email address is requiured when creating a director.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter a valid EmailAddress")]
        [StringLength(128, ErrorMessage = "An email must be less than 128 characters long.")]
        public string Email { get; set; }

        public bool IsActive { get; set; } = true;

        public string FullName { get { return NameSummary(); } }

        
        public ICollection<DirectorLocation> DirectorLocations { get; set; } = new HashSet<DirectorLocation>();

        public ICollection<AttendanceSheet> AttendanceSheets { get; set; } = new HashSet<AttendanceSheet>();


        public string NameSummary()
        {
            return $"{FirstName} {LastName}";
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(DirectorLocations.Count()  == 0)
            {
                yield return new ValidationResult("A director requires at least one location", ["DirectorLocations"]);
            }
        }
    }
}
