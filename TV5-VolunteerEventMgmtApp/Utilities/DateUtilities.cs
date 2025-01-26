namespace TV5_VolunteerEventMgmtApp.Utilities
{
    public static class DateUtilities
    {
        public static DateTime GetWeekStart() // monday of this week
        {
            DayOfWeek currentDay = DateTime.Now.DayOfWeek;


            var now = DateTime.Now;
            var today = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);

            if (currentDay == DayOfWeek.Monday)
            {
                return today;
            }
            if (currentDay == DayOfWeek.Tuesday)
            {
                return today.AddDays(-1);
            }
            if (currentDay == DayOfWeek.Wednesday)
            {
                return today.AddDays(-2);
            }
            if (currentDay == DayOfWeek.Thursday)
            {
                return today.AddDays(-3);
            }
            if (currentDay == DayOfWeek.Friday)
            {
                return today.AddDays(-4);
            }
            if (currentDay == DayOfWeek.Saturday)
            {
                return today.AddDays(-5);
            }
            if (currentDay == DayOfWeek.Sunday)
            {
                return today.AddDays(-6);
            }

            return today;
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
