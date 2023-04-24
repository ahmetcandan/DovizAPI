namespace DovizAPI
{
    public static class Helper
    {
        public static decimal? ToDecimal(this string value)
        {
            decimal result;
            value = value.Trim().Replace(".", "").Replace(",", ".");
            if (decimal.TryParse(value, out result))
                return result;
            return null;
        }
    }
}
