using System.ComponentModel.DataAnnotations;

namespace TV5_VolunteerEventMgmtApp.Models
{
    public class Singer : IValidatableObject
    {
        public enum ContactRelation
        {
            Mother,
            Father,
            Sibling,
            GrandParent,
            SocialWorker
        }

        public int Id { get; set; }

		[Display(Name = "Phone")]
		public string PhoneFormatted => "(" + Phone?.Substring(0, 3) + ") "
			+ Phone?.Substring(3, 3) + "-" + Phone?[6..];

		[Display(Name ="First Name")]
        [StringLength(128, ErrorMessage = "A first name must be less than 128 characters long.")]
        [Required(ErrorMessage = "Please enter a first name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please enter a last name")]
        [StringLength(128, ErrorMessage = "A last name must be less than 128 characters long.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Date of Birth")]
        [Required(ErrorMessage ="Please enter the singers DOB")]
        public DateOnly DOB { get; set; }

        [Display(Name = "Emergency Contact")]
        [StringLength(128, ErrorMessage = "Contact name must be less than 128 characters long.")]
        public string? EmergencyContactName { get; set; }

        [Display(Name = "Relation to Singer")]
        public ContactRelation? Relation { get; set; }

        [Display(Name ="Notes")]
        [MaxLength(500, ErrorMessage = "personal notes cannot exceed 500 characters.")]
        public string? Notes { get; set; }


        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage ="Please enter a valid email address.")]
        public string? Email { get; set; }
        [Display(Name = "Phone Number")]
        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        [StringLength(10, ErrorMessage = "A phone number must be 10 numbers long.", MinimumLength = 10)]
        [DataType(DataType.PhoneNumber)]
        public string? Phone {  get; set; }

        public bool isActive { get; set; } = true;

        public ICollection<Attendee> Attendance {  get; set; } = new HashSet<Attendee>();
        public ICollection<SingerLocation> SingerLocation { get; set; } = new HashSet<SingerLocation>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DOB > DateOnly.FromDateTime(DateTime.Now.AddYears(-8))){
                yield return new ValidationResult("Singers must be at least 8 years old.");
            }
            if (DOB < DateOnly.FromDateTime(DateTime.Now.AddYears(-18)))
            {
                yield return new ValidationResult("Singers cannot be over the age of 18.");
            }      
        }
    }
}
