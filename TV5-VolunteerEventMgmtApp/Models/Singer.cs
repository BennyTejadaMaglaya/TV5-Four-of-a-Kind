using System.ComponentModel.DataAnnotations;

namespace TV5_VolunteerEventMgmtApp.Models
{
    public class Singer
    {
        public int Id { get; set; }

		[Display(Name = "Phone")]
		public string PhoneFormatted => "(" + Phone?.Substring(0, 3) + ") "
			+ Phone?.Substring(3, 3) + "-" + Phone?[6..];

		[Display(Name ="First Name")]
        [Required(ErrorMessage = "Please enter a first name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please enter a last name")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Date of Birth")]
        [Required(ErrorMessage ="Please enter the singers DOB")]
        public DateOnly DOB { get; set; }

        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string? Phone {  get; set; }

        public ICollection<Attendee> Attendance {  get; set; } = new HashSet<Attendee>();

        [Display(Name = "Active")]
        public bool isActive { get; set; } = true;

        [Display(Name = "Location")]
        [Required(ErrorMessage = "Please select a location.")]
        public int LocationId { get; set; }
        public Location? Location { get; set; }
        public ICollection<SingerLocation> SingerLocation { get; set; } = new HashSet<SingerLocation>();
    }
}
