using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherSim.Services
{
    internal class WeatherGenerator
    {
        private static readonly (string weather, string iconPath) Snowy = ("Snowy", "/Assets/Icons/weather_snowy.png");
        private static readonly (string weather, string iconPath) Cloudy = ("Cloudy", "/Assets/Icons/weather_cloudy.png");
        private static readonly (string weather, string iconPath) Sunny = ("Sunny", "/Assets/Icons/weather_sunny.png");
        private static readonly (string weather, string iconPath) Fog = ("Fog", "/Assets/Icons/weather_fog.png");
        private static readonly (string weather, string iconPath) Storm = ("Storm", "/Assets/Icons/weather_storm.png");
        private static readonly (string weather, string iconPath) Rain = ("Rain", "/Assets/Icons/weather_rain.png");

        Random random = new Random();

        // Returns the season corresponding to a given month
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

        // Generates a temperature based on the season
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

        public int CelsiusToFahrenheit(int temp)
        {
            double conversion = (temp * 1.8) + 32;
            return (int)Math.Round(conversion);
        }

        // Generates random weather for a given season
        public (string weather, string iconPath) PickWeather(string season)
        {
            var snowy = Snowy;
            var cloudy = Cloudy;
            var sunny = Sunny;
            var fog = Fog;
            var storm = Storm;
            var rain = Rain;

            int roll = random.Next(100);

            return season switch
            {
                "winter" => roll switch
                {
                    < 30 => (Snowy),
                    < 60 => (Cloudy),
                    < 65 => (Sunny),
                    < 70 => (Fog),
                    < 80 => (Storm),
                    _ => (Rain)
                },

                "spring" => roll switch
                {
                    < 0 => (Snowy),
                    < 20 => (Cloudy),
                    < 50 => (Sunny),
                    < 70 => (Fog),
                    < 80 => (Storm),
                    _ => (Rain)
                },

                "summer" => roll switch
                {
                    < 0 => (Snowy),
                    < 20 => (Cloudy),
                    < 75 => (Sunny),
                    < 80 => (Fog),
                    < 90 => (Storm),
                    _ => (Rain)
                },

                "autumn" => roll switch
                {
                    < 1 => (Snowy),
                    < 30 => (Cloudy),
                    < 55 => (Sunny),
                    < 65 => (Fog),
                    < 80 => (Storm),
                    _ => (Rain)
                },

                _ => (Rain)
            };
        }
    }
}
