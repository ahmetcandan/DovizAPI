namespace DovizAPI.Model
{
    public class Currency
    {
        public Currency(string code)
        {
            Code = code;
        }
        public string Code { get; set; }
        public string Title { get; set; }
        public decimal? Buy { get; set; }
        public decimal? Sell { get; set; }
        public decimal? Max { get; set; }
        public decimal? Min { get; set; }
        public decimal? Rate { get; set; }
        public TimeSpan? Time { get; set; }
    }
}
