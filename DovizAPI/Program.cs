using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/rate/{code}", (string code) =>
{
    WebRequest request = WebRequest.Create("https://kur.doviz.com/");
    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
    Stream dataStream = response.GetResponseStream();
    StreamReader reader = new StreamReader(dataStream);
    string stringResponse = reader.ReadToEnd();
    Currency currency = currencyValue(ref stringResponse, code.ToUpper());
    reader.Close();
    dataStream.Close();
    response.Close();
    return currency == null ? new Currency($"Not Found '{code}' !") : currency;
})
.WithName("GetRate");

app.MapGet("/stock/{code}", (string code) =>
{
    WebRequest request = WebRequest.Create("https://borsa.doviz.com/hisseler");
    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
    Stream dataStream = response.GetResponseStream();
    StreamReader reader = new StreamReader(dataStream);
    string stringResponse = reader.ReadToEnd();
    Stock stock = stockValue(ref stringResponse, code.ToUpper());
    reader.Close();
    dataStream.Close();
    response.Close();
    return stock == null ? new Stock($"Not Found '{code}' !") : stock;
})
.WithName("GetStock");

app.Run();

Currency currencyValue(ref string text, string code)
{
    string searchKey = $@"<td class=""text-bold""  data-socket-key=""{code}"" data-socket-type=""C"" data-socket-attr=""b"" data-socket-animate=""true"" >";
    int startIndex = text.IndexOf(searchKey);
    if (startIndex == -1)
        return null;
    startIndex += searchKey.Length;
    int endIndex = text.IndexOf("</td>", startIndex);
    string bValue = text.Substring(startIndex, endIndex - startIndex).decimalConvert();

    int index1 = text.Substring(0, startIndex).LastIndexOf("<a href=");
    if (index1 == -1)
        return null;
    index1 = text.IndexOf("</span>", index1);
    if (index1 == -1)
        return null;
    index1 += "</span>".Length;
    int index2 = text.IndexOf("</a>", index1);
    if (index2 == -1)
        return null;
    string title = text.Substring(index1, index2 - index1).Trim();

    searchKey = $@"<td class=""text-bold""  data-socket-key=""{code}"" data-socket-type=""C"" data-socket-attr=""s"" data-socket-animate=""true"" >";
    startIndex = text.IndexOf(searchKey, endIndex);
    if (startIndex == -1)
        return null;
    startIndex += searchKey.Length;
    endIndex = text.IndexOf("</td>", startIndex);
    string sValue = text.Substring(startIndex, endIndex - startIndex).decimalConvert();

    searchKey = @"<td>";
    startIndex = text.IndexOf(searchKey, endIndex);
    if (startIndex == -1)
        return null;
    startIndex += searchKey.Length;
    endIndex = text.IndexOf("</td>", startIndex);
    string maxValue = text.Substring(startIndex, endIndex - startIndex).Replace("%", "").decimalConvert();

    searchKey = @"<td>";
    startIndex = text.IndexOf(searchKey, endIndex);
    if (startIndex == -1)
        return null;
    startIndex += searchKey.Length;
    endIndex = text.IndexOf("</td>", startIndex);
    string minValue = text.Substring(startIndex, endIndex - startIndex).Replace("%", "").decimalConvert();

    searchKey = @"data-socket-attr=""c"" >";
    startIndex = text.IndexOf(searchKey, endIndex);
    if (startIndex == -1)
        return null;
    startIndex += searchKey.Length;
    endIndex = text.IndexOf("</td>", startIndex);
    string cValue = text.Substring(startIndex, endIndex - startIndex).Replace("%", "").decimalConvert();

    searchKey = @"<td class=""time"">";
    startIndex = text.IndexOf(searchKey, endIndex);
    if (startIndex == -1)
        return null;
    startIndex += searchKey.Length;
    endIndex = text.IndexOf("</td>", startIndex);
    var tValue = text.Substring(startIndex, endIndex - startIndex).Trim().Split(":");

    return new Currency(code)
    {
        Buy = decimal.Parse(bValue),
        Sell = decimal.Parse(sValue),
        Max = decimal.Parse(maxValue),
        Min = decimal.Parse(minValue),
        Rate = decimal.Parse(cValue),
        Title = title,
        Time = new TimeSpan(int.Parse(tValue[0]), int.Parse(tValue[1]), 0)
    };
}

Stock stockValue(ref string text, string code)
{
    string searchKey = $@"<a href=""//borsa.doviz.com/hisseler/{code}"">";
    int startIndex = text.IndexOf(searchKey);
    if (startIndex == -1)
        return null;
    startIndex += searchKey.Length;
    int endIndex = text.IndexOf("</a>", startIndex);
    string title = text.Substring(startIndex, endIndex - startIndex).decimalConvert();

    searchKey = $@"<td class=""text-bold"">";
    startIndex = text.IndexOf(searchKey, endIndex);
    if (startIndex == -1)
        return null;
    startIndex += searchKey.Length;
    endIndex = text.IndexOf("</td>", startIndex);
    string lValue = text.Substring(startIndex, endIndex - startIndex).decimalConvert();

    searchKey = @"<td>";
    startIndex = text.IndexOf(searchKey, endIndex);
    if (startIndex == -1)
        return null;
    startIndex += searchKey.Length;
    endIndex = text.IndexOf("</td>", startIndex);
    string maxValue = text.Substring(startIndex, endIndex - startIndex).decimalConvert();

    searchKey = @"<td>";
    startIndex = text.IndexOf(searchKey, endIndex);
    if (startIndex == -1)
        return null;
    startIndex += searchKey.Length;
    endIndex = text.IndexOf("</td>", startIndex);
    string minValue = text.Substring(startIndex, endIndex - startIndex).decimalConvert();

    searchKey = @"<td>";
    startIndex = text.IndexOf(searchKey, endIndex);
    if (startIndex == -1)
        return null;
    startIndex += searchKey.Length;
    endIndex = text.IndexOf("</td>", startIndex);
    string vValue = text.Substring(startIndex, endIndex - startIndex).decimalConvert();

    searchKey = @"<td class=""text-bold color-";
    startIndex = text.IndexOf(searchKey, endIndex);
    if (startIndex == -1)
        return null;
    startIndex += searchKey.Length;
    startIndex = text.IndexOf(@""">", startIndex);
    if (startIndex == -1)
        return null;
    startIndex += 2;
    endIndex = text.IndexOf("</td>", startIndex);
    string cValue = text.Substring(startIndex, endIndex - startIndex).Trim().Replace("%", "").decimalConvert();

    searchKey = @"<td class=""time"">";
    startIndex = text.IndexOf(searchKey, endIndex);
    if (startIndex == -1)
        return null;
    startIndex += searchKey.Length;
    endIndex = text.IndexOf("</td>", startIndex);
    var tValue = text.Substring(startIndex, endIndex - startIndex).Trim().Split(":");

    return new Stock(code)
    {
        Last = decimal.Parse(lValue),
        Max = decimal.Parse(maxValue),
        Min = decimal.Parse(minValue),
        Rate = decimal.Parse(cValue),
        Volume = decimal.Parse(vValue),
        Title = title,
        Time = new TimeSpan(int.Parse(tValue[0]), int.Parse(tValue[1]), 0)
    };
}

static class Helper
{
    public static string decimalConvert(this string value)
    {
        return value.Trim().Replace(".", "").Replace(",", ".");
    }
}

class Currency
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

class Stock
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