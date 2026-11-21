namespace LibrarySystemWebAPI.Middlewares
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;

        private static readonly object _lock = new object();

        private static int _counter = 0;

        private static DateTime _windowStart = DateTime.UtcNow;

        public RateLimitingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            bool isAllowed;

            lock (_lock)
            {
                var now = DateTime.UtcNow;

                if ((now - _windowStart).TotalSeconds >= 10)
                {
                    _counter = 0;
                    _windowStart = now;
                }

                if (_counter < 10)
                {
                    _counter++;
                    isAllowed = true;
                }
                else
                {
                    isAllowed = false;
                }
            }

            if (!isAllowed)
            {
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;

                await context.Response.WriteAsync(
                    "Rate Limit Exceeded!");

                return;
            }

            await _next(context);
        }
    }
}