using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RevenueRecognitionSystem.DTOs;
using RevenueRecognitionSystem.Exceptions;
using RevenueRecognitionSystem.Services;

namespace RevenueRecognitionSystem.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ContractsController : ControllerBase
{
    private readonly IContractService _contractService;

    public ContractsController(IContractService contractService)
    {
        _contractService = contractService;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateContract(CancellationToken token, [FromBody] ContractRequestDto dto)
    {
        try
        {
            await _contractService.CreateContractAsync(token, dto);
            return Ok(new { message = "Contract created successfully. Awaiting for payment " });
        }catch (ValidationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (ConflictException ex)
        {
            return Conflict(new { error = ex.Message });
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }
}