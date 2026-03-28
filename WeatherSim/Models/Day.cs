using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherSim.Models
{
    public class Day
    {
        public DateTime Date { get; set; }
        public string? DisplayDate { get; set; }
        public string? ImagePath { get; set; }
        public string? Temperature { get; set; }
        public string? Weather { get; set; }
    }
}
