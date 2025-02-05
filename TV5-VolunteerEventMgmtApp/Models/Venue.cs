using System.ComponentModel.DataAnnotations;

namespace TV5_VolunteerEventMgmtApp.Models
{
    public class Venue
    {
        public int ID { get; set; }

        [Display(Name = "Venue Name")]
        [Required(ErrorMessage = "A Venue requires a name.")]
        [StringLength(128, ErrorMessage = "A Venue name must be less than 128 characters.")]
        public string VenueName { get; set; }
        public string? Description { get; set; }
        [Required(ErrorMessage = "An address is required for a venue.")]
        public string Address { get; set; }
        [Display(Name = "Name")]
        [StringLength(128, ErrorMessage = "Contact name must be less than 128 characters")]
        [Required(ErrorMessage = "A venue requires a contact name")]
        public string ContactName { get; set; }
        [Display(Name = "Phone")]
        [DataType(DataType.PhoneNumber)]
        public string? ContactPhone { get; set; }
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string? ContactEmail { get; set; }

        public bool IsActive { get; set; }

        // do we need this? think so?
        public int LocationId { get; set; }
        public Location? Location { get; set; }

        //This could be used to display the number of past events that have happened at this location.
        [Display(Name ="Successful Events")]
        public int NumPastEvents { get; set; }  

        //Events at this Venue likely ony display current and future
        [Display(Name ="Events")]
        public ICollection<VolunteerEvent> EventVenues { get; set; }

      
    }
}
