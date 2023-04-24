using System.Net;
using DovizAPI.Manager;
using DovizAPI.Model;

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
var dovizManager = new DovizComManager();
var mynetManager = new MynetManager();

app.MapGet("/rate/{code}", (string code) =>
{
    var currency = dovizManager.GetCurrency(code);
    return currency == null ? new Currency($"Not Found '{code}' !") : currency;
})
.WithName("GetRate");

app.MapGet("/stock/{code}", (string code) =>
{
    var stock = dovizManager.GetStock(code.ToUpper());
    return stock == null ? new Stock($"Not Found '{code}' !") : stock;
})
.WithName("GetStock");

app.MapGet("/fund/{code}", (string code) =>
{
    var stock = dovizManager.GetFund(code.ToUpper());
    return stock == null ? new Stock($"Not Found '{code}' !") : stock;
})
.WithName("GetFund");

app.MapGet("/weather/{city}", (string city) =>
{
    var weather = mynetManager.GetWeather(city.ToLower());
    return weather == null ? new Weather($"Not Found '{city}' !") : weather;
})
.WithName("GetWeather");

app.Run();
