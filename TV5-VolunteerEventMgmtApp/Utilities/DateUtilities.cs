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

        public static DateTime GetWeekStart(DateTime date)
        {
            DayOfWeek day = date.DayOfWeek;
            if (day == DayOfWeek.Monday)
            {
                return date;
            }
            int daysTillWeekStart =  DayOfWeek.Monday -day;
            DateTime currentWeekStartDate = date.AddDays(daysTillWeekStart);
            Console.WriteLine(currentWeekStartDate);
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

        public static DateTime GetWeekEnd(DateTime date) // friday in the same week of the date passed
        {
            // this could be more efficient but it is what it is 
            return GetWeekStart(date).AddDays(4);
        }
    }
}
