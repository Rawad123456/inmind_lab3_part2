// EnrollmentStructure/Middleware/TenantMiddleware.cs
using System.Threading.Tasks;
using EnrollmentStructure.Tenant;
using Microsoft.AspNetCore.Http;

namespace EnrollmentStructure.Middleware
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ITenantProvider tenantProvider)
        {
            if (context.Request.Headers.TryGetValue("X-Tenant-ID", out var tenantId))
            {
                tenantProvider.SetTenantId(tenantId!);
            }

            await _next(context);
        }
    }
}