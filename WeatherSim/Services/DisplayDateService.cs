using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherSim.Services
{
    public static class DisplayDateService
    {
        public static string GetDisplayDate(string selectedSeason, int index, DateTime date)
        {
            if (selectedSeason != "all")
                return date.ToString("MMM dd, yyyy");

            return index switch
            {
                0 => "Today",
                1 => "Tomorrow",
                _ => date.ToString("MMM dd, yyyy")
            };
        }
    }
}
