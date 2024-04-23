namespace ExchangeRateApp.Services
{
    public interface IExchangeRateProvider
    {
        Task<IEnumerable<ExchangeRate?>> GetExchangeRates(IEnumerable<Currency> currencies);
    }
}
