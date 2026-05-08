namespace SqlAcademy.Persistence.MultiTenancy;

public interface ISqlSessionContextAccessor
{
    int? TenantId { get; set; }

    bool BypassRowLevelSecurity { get; set; }
}