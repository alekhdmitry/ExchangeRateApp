namespace ExchangeRateApp
{
    public class ExchangeRate
    {
        public Currency SourceCurrency { get; set; }

        public Currency TargetCurrency { get; set; }
        public decimal Rate { get; set; }
        public int Amount { get; set; }

        public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal rate, int amount)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Rate           = rate;
            Amount         = amount;
        }

        public override string ToString()
        {
            return $"In {Amount} {TargetCurrency} is {Rate} {SourceCurrency}";
        }
    }
}