using MyOwnApi.Repositories;

namespace MyOwnApi.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string HEADER_NAME = "X-Api-Key";

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IApiKeyRepository apiKeyRepository)
        {
            var path = context.Request.Path.Value?.ToLower();

            // Protect only /api/notes endpoints
            if (path != null && path.StartsWith("/api/notes"))
            {
                // 1. read API key from header
                if (!context.Request.Headers.TryGetValue(HEADER_NAME, out var extractedKey))
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("API Key missing");
                    return;
                }

                // 2. Search for API key in database
                var apiKey = await apiKeyRepository.GetByKeyAsync(extractedKey!);

                if (apiKey == null)
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Invalid API Key");
                    return;
                }

                // 3. Verify expiration
                if (apiKey.ExpiresAt < DateTime.UtcNow)
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("API Key expired");
                    return;
                }

                // 4. add UserId to HttpContext
                context.Items["UserId"] = apiKey.UserId;
            }

            await _next(context);
        }
    }
}
