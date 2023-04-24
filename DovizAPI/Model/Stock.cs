namespace DovizAPI.Model
{
    public class Stock
    {
        public Stock(string code)
        {
            Code = code;
        }
        public string Code { get; set; }
        public string Title { get; set; }
        public decimal? Last { get; set; }
        public decimal? Max { get; set; }
        public decimal? Min { get; set; }
        public decimal? Rate { get; set; }
        public decimal? Volume { get; set; }
        public TimeSpan? Time { get; set; }
    }
}
