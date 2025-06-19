using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using RevenueRecognitionSystem.DTOs;
using RevenueRecognitionSystem.Exceptions;
using RevenueRecognitionSystem.Services;

namespace RevenueRecognitionSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(CancellationToken token, [FromBody] RegisterEmployeeDto dto)
    {
        try
        {
            await _authService.RegisterEmployeeAsync(token, dto);
            return Ok(new { message = "User registered successfully." });
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (ConflictException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(CancellationToken token, [FromBody] LoginRequestDto dto)
    {
        try
        {
            var result = await _authService.LoginEmployeeAsync(token, dto);
            return Ok(result);
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }
    
    
}