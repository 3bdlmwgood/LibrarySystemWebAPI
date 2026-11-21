namespace LibrarySystemWebAPI.Middlewares
{
    public class RequestIdHeaderMiddleware : IMiddleware
    {
        private readonly ILogger<RequestIdHeaderMiddleware> _logger;

        public RequestIdHeaderMiddleware(
            ILogger<RequestIdHeaderMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(
            HttpContext context,
            RequestDelegate next)
        {
            var requestId = context.TraceIdentifier;

            context.Response.Headers["X-Request-Id"] = requestId;

            _logger.LogDebug(
                "Request ID: {RequestId}, Method: {Method}, Path: {Path}",
                requestId,
                context.Request.Method,
                context.Request.Path);

            await next(context);
        }
    }
}