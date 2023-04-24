namespace DovizAPI.Model
{
    public class Weather
    {
        public Weather()
        {

        }

        public Weather(string city)
        {
            City = city;
        }

        public string City { get; set; }
        public string Temperature { get; set; }
        public string Info1 { get; set; }
        public string Info2 { get; set; }
        public string Info3 { get; set; }
        public string Info4 { get; set; }
        public string Info5 { get; set; }
        public string DateTime { get; set; }
    }
}
