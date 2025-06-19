using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using RevenueRecognitionSystem.DTOs;
using RevenueRecognitionSystem.DTOs.Response;
using RevenueRecognitionSystem.Exceptions;
using RevenueRecognitionSystem.Models;

namespace RevenueRecognitionSystem.Services;

public class AuthService : IAuthService
{
    
    private readonly AppDbContext _dbContext;
    
    public AuthService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task RegisterEmployeeAsync(CancellationToken token, RegisterEmployeeDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Login) || string.IsNullOrWhiteSpace(dto.Password))
        {
            throw new ValidationException("Login and password cannot be empty.");
        }

        if (await _dbContext.Employees.AnyAsync(e => e.Login == dto.Login, token))
        {
            throw new ConflictException($"Employee with login '{dto.Login}' already exists.");
        }
        
        var (hashedPassword, salt) = GetHashedPasswordAndSalt(dto.Password);

        var employee = new Employee()
        {
            Login = dto.Login,
            Password = hashedPassword,
            Salt = salt,
            Role = dto.Role,
            RefreshToken = "",
            RefreshTokenExp = null
        };

        _dbContext.Employees.Add(employee);
        await _dbContext.SaveChangesAsync(token);
    }
    
    public async Task<LoginResponseDto> LoginEmployeeAsync(CancellationToken token, LoginRequestDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Login) || string.IsNullOrWhiteSpace(dto.Password))
        {
            throw new ValidationException("Login and password cannot be empty.");
        }

        var employee = await _dbContext.Employees
            .FirstOrDefaultAsync(e => e.Login == dto.Login, token);

        if (employee == null)
        {
            throw new NotFoundException($"Employee with login '{dto.Login}' not found.");
        }

        string hashedPassword = GetHashedPasswordWithSalt(dto.Password, employee.Salt);
        
        if (hashedPassword != employee.Password)
        {
            throw new ValidationException("Invalid password.");
        }

        // Generate a new refresh token and set its expiration
        string refreshToken = Guid.NewGuid().ToString();
        employee.RefreshToken = refreshToken;
        employee.RefreshTokenExp = DateTime.UtcNow.AddDays(10); // 10 days)

        await _dbContext.SaveChangesAsync(token);

        return new LoginResponseDto()
        {
            Login = employee.Login,
            Role = employee.Role,
            RefreshToken = refreshToken
        };
    }
    
    
    private static Tuple<string, string> GetHashedPasswordAndSalt(string password)
    {
        byte[] salt = new byte[128 / 8];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }
        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));
        string saltBase64 = Convert.ToBase64String(salt);
        return new(hashed, saltBase64);
    }
    
    public static string GetHashedPasswordWithSalt(string password, string salt)
    {
        byte[] saltBytes = Convert.FromBase64String(salt);
        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: saltBytes,
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));

        return hashed;
    }
    
}