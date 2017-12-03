namespace RESTyr
{
    public class Weather
    {
        public string From { get; set; }
        public string To { get; set; }
        public int Temperature { get; set; }
        public double Precipitation { get; set; }
        public double Pressure { get; set; }

        public override string ToString()
        {
            return From + " " + To + " " + Temperature + " " + Pressure + " " + Precipitation;
        }
    }
}
