using System;
using System.Collections.Generic;
using System.Text;
using WeatherSim.Helpers;

namespace WeatherSim.Services
{
    public static class StatisticsService
    {
        public static string BuildWeatherCountText(Dictionary<string, int> counts)
        {
            var sb = new StringBuilder();

            foreach (var key in WeatherKeys.All)
                sb.AppendLine($"{key}: {counts[key]}");
            return sb.ToString();
        }
    }
}
