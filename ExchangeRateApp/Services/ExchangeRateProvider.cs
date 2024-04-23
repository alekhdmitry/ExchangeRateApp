using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using ExchangeRateApp.Services;
using ExchangeRateApp;
using System.Formats.Asn1;
using CsvHelper.Configuration;
using CsvHelper;
using CsvHelper.TypeConversion;

public class ExchangeRateProvider : IExchangeRateProvider
{
    private readonly HttpClient _httpClient;
    private const string CnbUrl = "https://www.cnb.cz/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/daily.txt";
    private readonly Currency BaseCurrency = new Currency("CZK");

    public ExchangeRateProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<ExchangeRate?>> GetExchangeRates(IEnumerable<Currency> currencies)
    {
        var response = await _httpClient.GetStringAsync(CnbUrl);

        var lines = response.Split(new[] { "\n" }, StringSplitOptions.None);

        var rates = new List<ExchangeRate>();
        var cnbRates = new List<CnbExchangeRate>();

        foreach (var line in lines.Skip(2)) // skip the header
        {
            try
            {
                var columns = line.Split('|');
                var rate = new CnbExchangeRate
                {
                    Country  = columns[0].Trim(),
                    Currency = columns[1].Trim(),
                    Amount   = int.Parse(columns[2].Trim(), CultureInfo.InvariantCulture),
                    Code     = columns[3].Trim(),
                    Rate     = decimal.Parse(columns[4].Trim(), CultureInfo.InvariantCulture)
                };

                var currency = new Currency(rate.Code);

                if (currencies.Any(c => c.Code == currency.Code))
                {
                    rates.Add(new ExchangeRate(BaseCurrency, currency, rate.Rate, rate.Amount));
                }
            }
            catch (FormatException fe)
            {
                Console.WriteLine($"Formatting error in the line '{line}': {fe.Message}");
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine($"Data missing in the line: {line}");
            }
        }

        return rates;
    }

    private class CnbExchangeRate
    {
        public string  Country { get; set; }
        public string  Currency { get; set; }
        public string  Code { get; set; }
        public int     Amount { get; set; }
        public decimal Rate { get; set; }
    }
}
