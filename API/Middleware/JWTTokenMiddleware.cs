namespace WealthFlow.API.Middleware
{
    public class JWTTokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<JWTTokenMiddleware> _logger;

        public JWTTokenMiddleware(RequestDelegate next, ILogger<JWTTokenMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
    }
}
