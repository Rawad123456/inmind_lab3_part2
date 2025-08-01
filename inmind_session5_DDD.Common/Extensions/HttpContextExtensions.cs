using Microsoft.AspNetCore.Http;

namespace Common.Extensions
{
    public static class HttpContextExtensions
    {
        public static string? GetTenantId(this HttpContext httpContext)
        {
            if (httpContext.Request.Headers.TryGetValue("X-Tenant-ID", out var tenantId))
            {
                return tenantId.ToString();
            }

            return null;
        }
    }
}