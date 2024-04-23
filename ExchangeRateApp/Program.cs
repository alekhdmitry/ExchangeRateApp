using ExchangeRateApp;
using ExchangeRateApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register the ExchangeRateProvider as a Scoped
builder.Services.AddScoped<IExchangeRateProvider, ExchangeRateProvider>();

// Add HttpClient support
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

//// --- TEST --- //

//var httpClient = new HttpClient();
//var provider = new ExchangeRateProvider(httpClient);
//var currencies = new List<Currency> { new Currency("USD"), new Currency("EUR") };

//try
//{
//    var rates = await provider.GetExchangeRates(currencies);
//    foreach (var rate in rates)
//    {
//        Console.WriteLine($"1 {rate.SourceCurrency.Code} = {rate.Rate} {rate.TargetCurrency.Code}");
//    }

//}
//catch (Exception ex)
//{
//    Console.WriteLine($"An error occurred: {ex.Message}");
//}
