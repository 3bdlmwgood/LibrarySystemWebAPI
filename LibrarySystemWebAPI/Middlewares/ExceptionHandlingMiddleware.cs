using LibrarySystemWebAPI.Errors;

namespace LibrarySystemWebAPI.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IHostEnvironment _environment;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger,
            IHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");

                if (context.Response.HasStarted)
                    throw;

                context.Response.Clear();
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/problem+json";

                context.Response.Headers["X-Request-Id"] =
                    context.TraceIdentifier;

                var problem = ExceptionProblemDetailsFactory.Create(
                    ex, context, _environment);

                await context.Response.WriteAsJsonAsync(problem);
            }
        }
    }
}