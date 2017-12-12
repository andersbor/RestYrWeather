using System;

namespace RESTyr
{
    public class Weather
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int Temperature { get; set; }
        public double Precipitation { get; set; }
        public double Pressure { get; set; }

        public override string ToString()
        {
            return From + " " + To + " " + Temperature + " " + Pressure + " " + Precipitation;
        }
    }
}
