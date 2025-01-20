namespace TV5_VolunteerEventMgmtApp.Utilities
{
    public static class DateUtilities
    {
        public static DateTime GetWeekStart() // monday of this week
        {
            DayOfWeek currentDay = DateTime.Now.DayOfWeek;
            int daysTillCurrentDay = currentDay - DayOfWeek.Monday; //change DayOfWeek.Day to set the start 
            DateTime currentWeekStartDate = DateTime.Now.AddDays(-daysTillCurrentDay);
            return currentWeekStartDate;
        }

        public static DateTime GetWeekEnd() // friday of this week
        {
            //DayOfWeek currentDay = DateTime.Now.DayOfWeek;
            //int daysTillCurrentDay = currentDay - DayOfWeek.Monday; //change DayOfWeek.Day to set the start 
            //DateTime currentWeekStartDate = DateTime.Now.AddDays(-daysTillCurrentDay);
            var currentWeekStartDate = GetWeekStart().AddDays(4); 
            return currentWeekStartDate;
        }
    }
}
