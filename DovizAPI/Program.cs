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
    Currency currency = new Currency(code);
    currency.Buyy = searchRate(ref stringResponse, currency.Code, "b");
    if (currency.Buyy == -1)
    {
        currency.Code = "Not Found !";
        currency.Buyy = 0;
        return currency;
    }
    currency.Sell = searchRate(ref stringResponse, currency.Code, "s");
    reader.Close();
    dataStream.Close();
    response.Close();
    return currency;
})
.WithName("GetRate");

app.Run();

decimal searchRate(ref string text, string currencyCode, string attr)
{
    string searchKey = $@"<td class=""text-bold""  data-socket-key=""{currencyCode}"" data-socket-type=""C"" data-socket-attr=""{attr}"" data-socket-animate=""true"" >";
    int startIndex = text.IndexOf(searchKey);
    if (startIndex == -1)
        return -1;
    startIndex += searchKey.Length;
    int length = text.Substring(startIndex).IndexOf("</td>");
    return Convert.ToDecimal(text.Substring(startIndex, length).Replace(",", "."));
}

class Currency
{
    public Currency(string code)
    {
        Code = code.ToUpper();
        DateTime = new DateTime();
    }
    public string Code { get; set; }
    public decimal Buyy { get; set; }
    public decimal Sell { get; set; }
    public DateTime DateTime { get; set; }
}