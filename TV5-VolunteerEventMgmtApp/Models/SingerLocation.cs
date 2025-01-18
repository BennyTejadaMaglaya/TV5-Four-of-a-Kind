namespace TV5_VolunteerEventMgmtApp.Models
{
    public class SingerLocation
    {
        public int SingerId { get; set; }
        public Singer? Singer { get; set; }

        public int LocationId { get; set; }
        public Location? Location { get; set; }
    }
}
