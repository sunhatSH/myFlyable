using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Flyable.Middleware
{
    public class SecurityHeadersMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            // 添加安全头
            context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
            context.Response.Headers.Append("X-Frame-Options", "DENY");
            context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
            context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
            
            // HTTPS重定向
            if (!context.Request.IsHttps && !context.Request.Host.Host.Contains("localhost"))
            {
                var httpsUrl = $"https://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";
                context.Response.Redirect(httpsUrl, permanent: true);
                return;
            }

            await next(context);
        }
    }
}