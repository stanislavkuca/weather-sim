using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherSim.Helpers
{
    public static class SeasonHelper
    {
        public static bool IsMonthInSeason(int month, string season)
        {
            return season switch
            {
                "winter" => month is 12 or 1 or 2,
                "spring" => month is 3 and <= 5,
                "summer" => month is 6 and <= 8,
                "autumn" => month is 9 and <= 11,
                _ => true
            };
        }

        public static DateTime AlignStartDateToSeason(DateTime date, string season)
        {
            if (season == "all")
                return date;

            while (!IsMonthInSeason(date.Month, season))
                date = date.AddDays(1);

            return date;
        }
    }
}
