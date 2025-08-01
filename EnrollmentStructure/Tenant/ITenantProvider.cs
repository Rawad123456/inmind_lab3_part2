namespace EnrollmentStructure.Tenant;

public interface ITenantProvider
{
    string GetTenantId();
     void SetTenantId(string tenantId);
    
}