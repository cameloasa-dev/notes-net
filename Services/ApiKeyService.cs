using MyOwnApi.DTOs;
using MyOwnApi.Exceptions;
using MyOwnApi.Models;
using MyOwnApi.Repositories;
using MyOwnApi.Validators;
using FluentValidation;

namespace MyOwnApi.Services;

     public interface IApiKeyService
    {
        Task<ApiKeyResponseDto> GenerateApiKeyAsync(string userId);
        Task DeleteApiKeyAsync(string userId);
        Task<ApiKeyResponseDto> GetApiKeyAsync(string userId);
    
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

        // ---------------------------
        // GENERATE API KEY
        // ---------------------------
        public async Task<ApiKeyResponseDto> GenerateApiKeyAsync(string userId)
        {
            // 1. Validate userId using FluentValidation
            var validation = await _apiKeyValidator.ValidateAsync(userId);

            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            // 2. Create new key
            var newKey = new ApiKey
            {
                Key = Guid.NewGuid().ToString("N"),
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
    
            };

            // 3. Save in DB
            await _apiKeyRepository.AddAsync(newKey);

            // 4. Return DTO
            return new ApiKeyResponseDto
            {
                ApiKey = newKey.Key,
                CreatedAt = newKey.CreatedAt,
                UserId = newKey.UserId
            };
        }

        // ---------------------------
        // DELETE API KEY
        // ---------------------------
        public async Task DeleteApiKeyAsync(string userId)
        {
            var existingKey = await _apiKeyRepository.GetByUserIdAsync(userId);

            if (existingKey == null)
                throw new NotFoundException("No API key found to delete.");

            await _apiKeyRepository.DeleteByUserIdAsync(userId);
        }

        // ---------------------------
        // Get API KEY
        // ---------------------------
        public async Task<ApiKeyResponseDto> GetApiKeyAsync(string userId)
        {
            var apiKey = await _apiKeyRepository.GetByUserIdAsync(userId);

            if (apiKey == null)
                throw new NotFoundException("No API key found.");

            return new ApiKeyResponseDto
            {
                ApiKey = apiKey.Key,
                CreatedAt = apiKey.CreatedAt,
                UserId = apiKey.UserId
            };
        }
    }
    

