using System.ComponentModel.DataAnnotations;

namespace TV5_VolunteerEventMgmtApp.Models
{
    public class Singer
    {
        public int Id { get; set; }
 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly DOB { get; set; }

        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string? Phone {  get; set; }

        public ICollection<Attendee> Attendance {  get; set; } = new HashSet<Attendee>();
        
        bool isActive { get; set; } = true;
    }
}
