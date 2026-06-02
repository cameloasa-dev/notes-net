using Microsoft.EntityFrameworkCore;
using MyOwnApi.Context;
using MyOwnApi.Models;

namespace MyOwnApi.Repositories
{
    public interface IApiKeyRepository
    {
        Task<ApiKey?> GetByUserIdAsync(string userId);
        Task<ApiKey?> GetByKeyAsync(string key);
        Task AddAsync(ApiKey apiKey);
    }

    public class ApiKeyRepository : IApiKeyRepository
    {
        private readonly ApplicationDbContext _context;

        public ApiKeyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ApiKey?> GetByUserIdAsync(string userId)
        {
            return await _context.ApiKeys
                .FirstOrDefaultAsync(k => k.UserId == userId);
        }

        public async Task<ApiKey?> GetByKeyAsync(string key)
        {
            return await _context.ApiKeys
                .FirstOrDefaultAsync(k => k.Key == key);
        }

        public async Task AddAsync(ApiKey apiKey)
        {
            _context.ApiKeys.Add(apiKey);
            await _context.SaveChangesAsync();
        }
    }
}
