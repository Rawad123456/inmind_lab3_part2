namespace inmind_session5_DDD.Common.Tenant;



public interface ITenantProvider
{
    string? TenantId { get; set; }
}
