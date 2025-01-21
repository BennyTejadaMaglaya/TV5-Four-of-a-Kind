namespace TV5_VolunteerEventMgmtApp.Utilities
{
    public class OpacityUtility
    {
        public string HexWithOpacity { get; set; }
        public OpacityUtility(string hexCode, float opacity)
        {
            int alphaValue = (int)(opacity * 255);
            HexWithOpacity = $"{hexCode}{alphaValue:X2}";
        }
    }
}
