using RevenueRecognitionSystem.Repositories;

namespace RevenueRecognitionSystem.Services;

public class RevenueService : IRevenueService
{
    private readonly PaymentRepository _paymentRepository;
    private readonly ContractRepository _contractRepository;
    private readonly ICurrencyExchangeService _currencyService;

    public RevenueService(PaymentRepository paymentRepository, ContractRepository contractRepository, ICurrencyExchangeService currencyService)
    {
        _paymentRepository = paymentRepository;
        _contractRepository = contractRepository;
        _currencyService = currencyService;
    }
    
    public async Task<decimal> GetCurrentRevenueAsync(string? currency, CancellationToken token)
    {
        var revenueInPLN = await _paymentRepository.GetTotalConfirmedRevenueAsync(token);

        if (!string.IsNullOrWhiteSpace(currency) && currency.ToUpper() != "PLN")
        {
            var rate = await _currencyService.GetExchangeRateAsync("PLN", currency.ToUpper(), token);
            revenueInPLN *= rate;
        }

        return Math.Round(revenueInPLN, 2);
    }
    
    public async Task<decimal> GetPredictedRevenueAsync(string? currency, CancellationToken token)
    {
        var currentRevenue = await _paymentRepository.GetTotalConfirmedRevenueAsync(token);
        var unsignedContracts = await _contractRepository.GetUnsignedContractsTotalValueAsync(token);

        var predictedRevenue = currentRevenue + unsignedContracts;

        if (!string.IsNullOrWhiteSpace(currency) && currency.ToUpper() != "PLN")
        {
            var rate = await _currencyService.GetExchangeRateAsync("PLN", currency.ToUpper(), token);
            predictedRevenue *= rate;
        }

        return Math.Round(predictedRevenue, 2);
    }
    
    
}