using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherSim.Services
{
    public static class StatisticsService
    {
        public static string BuildWeatherCountText(Dictionary<string, int> counts)
        {
            var sb = new StringBuilder();
            foreach (var kvp in counts)
                sb.AppendLine($"{kvp.Key}: {kvp.Value}");
            return sb.ToString();
        }
    }
}
