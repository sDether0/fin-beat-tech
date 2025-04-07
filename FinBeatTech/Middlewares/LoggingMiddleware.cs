namespace FinBeatTech.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if(context.Request.Path.StartsWithSegments("/health"))
            {
                _logger.LogInformation("Health check request: {0} {1}", context.Request.Method, context.Request.Path);
                await _next(context);
                return;
            }
            if (context.Request.Path.StartsWithSegments("/swagger"))
            {
                _logger.LogInformation("Swagger request: {0} {1}", context.Request.Method, context.Request.Path);
                await _next(context);
                return;
            }
            using (_logger.BeginScope("ApiLM"))
            {
                _logger.LogInformation("Request: {0} {1}", context.Request.Method, context.Request.Path);

                await _next(context);

                _logger.LogInformation("Response: {0}", context.Response.StatusCode);
            }
        }
    }
}
