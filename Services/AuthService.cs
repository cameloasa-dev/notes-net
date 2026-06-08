using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MyOwnApi.DTOs;
using MyOwnApi.Exceptions;
using MyOwnApi.Models;
using MyOwnApi.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyOwnApi.Services;

public interface IAuthService
    {
        Task<IdentityResult> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
    }

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly SignInManager<User> _signInManager;
    private readonly IConfiguration _config;
    private readonly IValidator<RegisterDto> _registerValidator;
    private readonly IValidator<LoginDto> _loginValidator;


    public AuthService(
        IUserRepository userRepository,
        SignInManager<User> signInManager,
        IConfiguration config,
        IValidator<RegisterDto> registerValidator,
        IValidator<LoginDto> loginValidator)
    {
        _userRepository = userRepository;
        _signInManager = signInManager;
        _config = config;
        _registerValidator = registerValidator;
        _loginValidator = loginValidator;
    }

    // ---------------------------
    // REGISTER
    // ---------------------------
    public async Task<IdentityResult> RegisterAsync(RegisterDto dto)
    {
        // 1. Validate DTO
        var validation = await _registerValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            throw new ValidationException(validation.Errors);

        // 2. Check if email exists
        var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
        if (existingUser != null)
            throw new ConflictException("Email already exists.");

        // 3. Create user
        var user = new User
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            UserName = dto.Email
        };

        return await _userRepository.CreateAsync(user, dto.Password);
        }

    // ---------------------------
    // LOGIN
    // ---------------------------
    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        // 1. Validate DTO
        var validation = await _loginValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            throw new ValidationException(validation.Errors);

        // 2. Check if user exists
        var user = await _userRepository.GetByEmailAsync(dto.Email);
        if (user == null)
            throw new NotFoundException("User not found.");

        // 3. Check password
        var passwordValid = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
        if (!passwordValid.Succeeded)
            throw new UnauthorizedException("Invalid email or password.");

        // 4. Generate token
        var token = GenerateJwtToken(user);

        return new AuthResponseDto
        {
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddHours(2),
            UserId = user.Id,
            Email = user.Email!,
            FirstName = user.FirstName,
            LastName = user.LastName
        };
    }

    // ---------------------------
    // GENERATE JWT TOKEN
    // ---------------------------
    private string GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new("firstName", user.FirstName),
            new("lastName", user.LastName)
        };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
