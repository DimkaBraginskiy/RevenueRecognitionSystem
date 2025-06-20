namespace RevenueRecognitionSystem.Services;

public interface ICurrencyExchangeService
{
    Task<decimal> GetExchangeRateAsync(string fromCurrency, string toCurrency, CancellationToken token);
}