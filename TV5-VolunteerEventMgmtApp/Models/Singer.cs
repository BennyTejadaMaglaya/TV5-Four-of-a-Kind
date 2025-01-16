using System.ComponentModel.DataAnnotations;

namespace TV5_VolunteerEventMgmtApp.Models
{
    public class Singer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly DOB { get; set; }

        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string? Phone {  get; set; }

        bool isActive { get; set; } = true;
    }
}
