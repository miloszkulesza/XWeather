using System;
using System.Collections.Generic;
using System.Text;

namespace XWeather.Models
{
    public class Weather
    {
        public DateTime Date { get; set; }
        public DateTime Sunrise { get; set; }
        public DateTime Sunset { get; set; }
        public double Temperature { get; set; }
        public double Pressure { get; set; }
        public double Wind { get; set; }
    }
}
