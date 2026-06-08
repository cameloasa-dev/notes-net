using MyOwnApi.Models;
using MyOwnApi.Repositories;
using MyOwnApi.Validators;
using MyOwnApi.DTOs;

namespace MyOwnApi.Services
{
    public interface IApiKeyService
    {
        Task<ApiKeyResponseDto> GenerateApiKeyAsync(string userId);
        Task DeleteApiKeyAsync(string userId);
        Task<ApiKeyResponseDto?> GetActiveApiKeyAsync(string userId);
    }

    public class ApiKeyService : IApiKeyService
    {
        private readonly IApiKeyRepository _apiKeyRepository;
        private readonly ApiKeyValidator _apiKeyValidator;

        public ApiKeyService(
            IApiKeyRepository apiKeyRepository,
            ApiKeyValidator apiKeyValidator)
        {
            _apiKeyRepository = apiKeyRepository;
            _apiKeyValidator = apiKeyValidator;
        }

        public async Task<ApiKeyResponseDto> GenerateApiKeyAsync(string userId)
        {
            // 1. Validate userId 
            var validation = await _apiKeyValidator.ValidateAsync(userId);

            if (!validation.IsValid)
                throw new Exception(validation.Errors.First().ErrorMessage);

            // 2. Generate the key
            var newKey = new ApiKey
            {
                Key = Guid.NewGuid().ToString("N"),
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMonths(6)
            };

            // 3. Save in database
            await _apiKeyRepository.AddAsync(newKey);

            // 4. Return the key to the user
            return new ApiKeyResponseDto
            {
                ApiKey = newKey.Key,
                ExpiresAt = newKey.ExpiresAt,
                CreatedAt = newKey.CreatedAt,
                UserId = newKey.UserId
            };
        }

        public async Task DeleteApiKeyAsync(string userId)
        {
            await _apiKeyRepository.DeleteByUserIdAsync(userId);
        }

        public async Task<ApiKeyResponseDto?> GetActiveApiKeyAsync(string userId)
        {
            var apiKey = await _apiKeyRepository.GetActiveApiKeyAsync(userId);

            if (apiKey == null)
                return null;

            return new ApiKeyResponseDto
            {
                ApiKey = apiKey.Key,
                CreatedAt = apiKey.CreatedAt,
                ExpiresAt = apiKey.ExpiresAt,
                UserId = apiKey.UserId
            };
        }
    }
}
