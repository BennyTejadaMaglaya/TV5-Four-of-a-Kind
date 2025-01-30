using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.RegularExpressions;

namespace TV5_VolunteerEventMgmtApp.Utilities
{
    public static class ValidationUtilities
    {
        public static bool IsPhoneNumber(string phoneNumber)
        {
            return Regex.Match(phoneNumber, @"(?<!\d)\d{10}(?!\d)").Success;
        }
    }
}
