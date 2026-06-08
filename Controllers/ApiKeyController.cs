using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MyOwnApi.Services;

namespace MyOwnApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiKeyController : ControllerBase
    {
        private readonly IApiKeyService _apiKeyService;

        public ApiKeyController(IApiKeyService apiKeyService)
        {
            _apiKeyService = apiKeyService;
        }

        // ---------------------------
        // GENERATE API KEY
        // ---------------------------
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GenerateApiKey()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not found.");

            var result = await _apiKeyService.GenerateApiKeyAsync(userId);

            return Ok(result);
        }
    
        // ---------------------------
        // DELETE API KEY
        // ---------------------------
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteApiKey()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not found.");

            await _apiKeyService.DeleteApiKeyAsync(userId);

            return Ok(new { message = "API Key deleted successfully" });
        }

        // ---------------------------
        // GET My API KEY
        // ---------------------------
        [HttpGet]
        public async Task<IActionResult> GetMyApiKey()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not authenticated." });

            var apiKey = await _apiKeyService.GetActiveApiKeyAsync(userId);

            if (apiKey == null)
                return NotFound(new { message = "No active API key found." });

            return Ok(apiKey);
        }

    }   
}
