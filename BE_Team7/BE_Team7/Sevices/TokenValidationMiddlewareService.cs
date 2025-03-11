namespace BE_Team7.Sevices
{
    public class TokenValidationMiddlewareService
    {
        private readonly RequestDelegate _next;

        public TokenValidationMiddlewareService(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (!string.IsNullOrEmpty(token))
            {
                var isRevoked = TokenRevocationService.IsTokenRevoked(token);
                if (isRevoked)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Token đã bị vô hiệu hóa");
                    return;
                }
            }

            await _next(context);
        }


    }
}