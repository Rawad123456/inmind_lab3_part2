using EnrollmentStructure.Tenant;

public class TenantProvider : ITenantProvider
{
    private string? _tenantId;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TenantProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetTenantId()
    {
        if (!string.IsNullOrWhiteSpace(_tenantId))
            return _tenantId!;

        return _httpContextAccessor.HttpContext?.Request.Headers["X-Tenant-ID"].FirstOrDefault() ?? string.Empty;
    }

    public void SetTenantId(string tenantId)
    {
        _tenantId = tenantId;
    }
}