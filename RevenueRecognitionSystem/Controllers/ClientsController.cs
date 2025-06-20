using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RevenueRecognitionSystem.DTOs;
using RevenueRecognitionSystem.Exceptions;
using RevenueRecognitionSystem.Services;

namespace RevenueRecognitionSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly IClientsService _clientsService;
    
    public ClientsController(IClientsService clientsService)
    {
        _clientsService = clientsService;
    }
    
    [HttpPut("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> UpdateClientAsync(CancellationToken token, int id, [FromBody] UpdateClientRequestDto dto)
    {
        try
        {
            await _clientsService.UpdateClientAsync(token, id, dto);
            return Ok(new { message = "Client updated successfully." });
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
    
    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteClientAsync(CancellationToken token, int id)
    {
        try
        {
            await _clientsService.DeleteClientAsync(token, id);
            return Ok(new { message = "Client deleted successfully." });
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddClientAsync(CancellationToken token, [FromBody] AddClientRequestDto dto)
    {
        try
        {
            await _clientsService.AddClientAsync(token, dto);
            return Ok(new { message = "Client added successfully." });
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
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetClientsAsync(CancellationToken token)
    {
        try
        {
            var clients = await _clientsService.GetClientsAsync(token);
            return Ok(clients);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
        }
    }
    
    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetClientByIdAsync(CancellationToken token, int id)
    {
        try
        {
            var client = await _clientsService.GetClientByIdAsync(token, id);
            return Ok(client);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "An unexpected error occurred.", details = ex.Message });
        }
    }
}