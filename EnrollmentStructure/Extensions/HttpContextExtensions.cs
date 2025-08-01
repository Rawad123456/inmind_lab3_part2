using Microsoft.AspNetCore.Http;

namespace EnrollmentStructure.Extensions
{
    public static class HttpContextExtensions
    {
        public static string? GetTenantId(this HttpContext context)
        {
            if (context.Request.Headers.TryGetValue("X-Tenant-ID", out var tenantId))
            {
                return tenantId.ToString();
            }

            return null;
        }
    }
}