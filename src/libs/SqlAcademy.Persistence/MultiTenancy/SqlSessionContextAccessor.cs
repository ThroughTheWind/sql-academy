namespace SqlAcademy.Persistence.MultiTenancy;

public sealed class SqlSessionContextAccessor : ISqlSessionContextAccessor
{
    public int? TenantId { get; set; }

    public bool BypassRowLevelSecurity { get; set; }
}