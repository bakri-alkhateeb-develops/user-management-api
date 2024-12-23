namespace user_management_api.Middlewares {
    public class AuthenticationMiddleware {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next) {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context) {
            if (!context.Request.Headers.TryGetValue("Authorization", out var token) || token != "valid_token") {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }
            await _next(context);
        }
    }
}