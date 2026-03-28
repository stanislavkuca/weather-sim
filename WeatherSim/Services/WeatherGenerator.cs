using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherSim.Services
{
    internal class WeatherGenerator
    {
        Random random = new Random();

        public string GetSeason(DateTime date)
        {
            int month = date.Month;

            return month switch
            {
                12 or 1 or 2 => "winter",
                3 or 4 or 5 => "spring",
                6 or 7 or 8 => "summer",
                _ => "autumn"
            };
        }

        public int GenerateTemperature(string season)
        {
            return season switch
            {
                "winter" => random.Next(-10, 6),
                "spring" => random.Next(5, 18),
                "summer" => random.Next(18, 36),
                "autumn" => random.Next(5, 15),
                _ => 0
            };
        }

        public (string weather, string iconPath) PickWeather(string season)
        {
            var cloudy = ("Cloudy", "/Assets/Icons/weather_cloudy.png");
            var snowy = ("Snowy", "/Assets/Icons/weather_snowy.png");
            var sunny = ("Sunny", "/Assets/Icons/weather_sunny.png");
            var fog = ("Fog", "/Assets/Icons/weather_fog.png");
            var storm = ("Storm", "/Assets/Icons/weather_storm.png");
            var rain = ("Rain", "/Assets/Icons/weather_rain.png");

            int roll = random.Next(100);

            return season switch
            {
                "winter" => roll switch
                {
                    < 30 => (cloudy),
                    < 70 => (snowy),
                    < 75 => (fog),
                    < 85 => (rain),
                    _ => (sunny)
                },

                "summer" => roll switch
                {
                    < 5 => (fog),
                    < 50 => (sunny),
                    < 60 => (storm),
                    < 90 => (cloudy),
                    _ => (rain),
                },

                _ => (cloudy)
            };
        }
    }
}
