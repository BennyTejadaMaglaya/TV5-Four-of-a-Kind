namespace TV5_VolunteerEventMgmtApp.Utilities
{
    public static class SummaryUtilities
    {
        

        public static string DateRangeMessage(DateTime? start, DateTime? end)
        {

            // if only end return "Before Date"
            // if only start return "After Date"
            // if both return "Between Date1 & Date2"

            if (start.HasValue && end.HasValue)
            {
                return $"Between {start.Value} & {end.Value}";
            }
            if (start.HasValue && !end.HasValue)
            {
                return $"After {start.Value}";
            }
            if (end.HasValue && !start.HasValue)
            {
                return $"Before {end.Value}";
            }
            return "";
        }
    }
}
