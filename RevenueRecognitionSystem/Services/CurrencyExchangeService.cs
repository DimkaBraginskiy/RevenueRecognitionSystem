using System.ComponentModel.DataAnnotations;

namespace RevenueRecognitionSystem.Services;

public class CurrencyExchangeService : ICurrencyExchangeService
{
    private readonly HttpClient _httpClient;

    public CurrencyExchangeService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<decimal> GetExchangeRateAsync(string fromCurrency, string toCurrency, CancellationToken token)
    {
        var url = $"https://api.exchangerate.host/latest?base={fromCurrency}&symbols={toCurrency}";
        var response = await _httpClient.GetFromJsonAsync<ExchangeRateResponse>(url, cancellationToken: token);

        if (response is null || !response.Rates.TryGetValue(toCurrency, out var rate))
        {
            throw new ValidationException($"Exchange rate from {fromCurrency} to {toCurrency} not found.");
        }

        return rate;
    }

    private class ExchangeRateResponse
    {
        public Dictionary<string, decimal> Rates { get; set; } = new();
    }
}