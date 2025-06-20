using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RevenueRecognitionSystem.Services;

namespace RevenueRecognitionSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RevenueController : ControllerBase
{
    private readonly IRevenueService _revenueService;

    public RevenueController(IRevenueService revenueService)
    {
        _revenueService = revenueService;
    }
    
    [HttpGet("current")]
    public async Task<IActionResult> GetCurrentRevenue([FromQuery] string? currency, CancellationToken token)
    {
        var revenue = await _revenueService.GetCurrentRevenueAsync(currency, token);
        return Ok(new { revenue });
    }

    [HttpGet("predicted")]
    public async Task<IActionResult> GetPredictedRevenue([FromQuery] string? currency, CancellationToken token)
    {
        var revenue = await _revenueService.GetPredictedRevenueAsync(currency, token);
        return Ok(new { revenue });
    }
}