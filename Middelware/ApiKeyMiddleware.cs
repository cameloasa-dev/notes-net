using MyOwnApi.Repositories;

namespace MyOwnApi.Middleware;

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

            // Protect ONLY /api/notes endpoints
            if (path != null && path.StartsWith("/api/notes"))
            {
                //  Public access for GET
                if (context.Request.Method == "GET" && !path.Contains("/mine"))
                {
                    await _next(context);
                    return;
                }

                // Protected access for  GET, POST, PUT, DELETE
                if (!context.Request.Headers.TryGetValue(HEADER_NAME, out var extractedKey))
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("API Key missing");
                    return;
                }

                var apiKey = await apiKeyRepository.GetByKeyAsync(extractedKey!);

                if (apiKey == null)
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Invalid API Key");
                    return;
                }

                // Attach UserId for create/update/delete
                context.Items["UserId"] = apiKey.UserId;
            }

            await _next(context);
        }
    }

