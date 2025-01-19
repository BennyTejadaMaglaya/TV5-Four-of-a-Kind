using System.ComponentModel.DataAnnotations;

namespace TV5_VolunteerEventMgmtApp.Models
{
    public class Director
    {
        public int ID { get; set; }

		[Display(Name = "Director")]
		[DisplayFormat(NullDisplayText = "None")]
		public string FullName
		{
			get
			{
				return FirstName + " " + LastName;
			}
		}

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
        [DataType(DataType.PhoneNumber, ErrorMessage = "Please enter a valid phone number")]
        public string PhoneNumber { get; set; }

        [Display( Name = "Email Address")]
        [Required(ErrorMessage = "An email address is requiured when creating a director.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter a valid EmailAddress")]
        public string Email { get; set; }

        public ICollection<DirectorLocation> DirectorLocations { get; set; } = new HashSet<DirectorLocation>();
    }
}
