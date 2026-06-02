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
    }
}
