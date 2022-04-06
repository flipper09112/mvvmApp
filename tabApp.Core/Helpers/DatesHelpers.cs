using System;
using System.Collections.Generic;
using System.Text;

namespace tabApp.Core.Helpers
{
    public static class DatesHelpers
    {
        public static DateTime NextSunday(this DateTime from)
        {
            if(from.DayOfWeek == DayOfWeek.Sunday)
                return from;

            DateTime date = from;
            int start = (int)from.DayOfWeek;
            int target = (int)DayOfWeek.Sunday;
            if (target <= start)
                target += 7;
            return date.AddDays(target - start);
        }

        public static DateTime GetFirstDayOfWeek(DateTime dayInWeek)
        {
            return GetFirstDayOfWeek(dayInWeek, DayOfWeek.Monday);
        }

        private static DateTime GetFirstDayOfWeek(DateTime dayInWeek, DayOfWeek firstDay)
        {
            DateTime firstDayInWeek = dayInWeek.Date;

            while (firstDayInWeek.DayOfWeek != firstDay)
            {
                firstDayInWeek = firstDayInWeek.AddDays(-1);
            }

            return firstDayInWeek;
        }
    }
}
