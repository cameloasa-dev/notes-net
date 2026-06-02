using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MyOwnApi.DTOs;
using MyOwnApi.Models;
using MyOwnApi.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyOwnApi.Services;

    public interface IAuthService
    {
        Task<IdentityResult> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto?> LoginAsync(LoginDto dto);
    }

    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _config;

        public AuthService(
            IUserRepository userRepository,
            SignInManager<User> signInManager,
            IConfiguration config)
        {
            _userRepository = userRepository;
            _signInManager = signInManager;
            _config = config;
        }

        // ---------------------------
        // REGISTER
        // ---------------------------
        public async Task<IdentityResult> RegisterAsync(RegisterDto dto)
        {
            var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
            if (existingUser != null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "Email already exists."
                });
            }

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
        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null)
                return null;

            var passwordValid = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if (!passwordValid.Succeeded)
                return null;

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

