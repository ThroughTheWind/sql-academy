using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SqlAcademy.Persistence.Database;
using SqlAcademy.Persistence.MultiTenancy;

namespace SqlAcademy.Persistence.Initialization;

public sealed class LearningDbInitializer(
    LearningDbContext dbContext,
    ISqlSessionContextAccessor sessionContextAccessor,
    ILogger<LearningDbInitializer> logger)
{
    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        var originalTenantId = sessionContextAccessor.TenantId;
        var originalBypass = sessionContextAccessor.BypassRowLevelSecurity;

        sessionContextAccessor.TenantId = null;
        sessionContextAccessor.BypassRowLevelSecurity = true;

        // Applying migrations at startup keeps local runs, container runs, and tests aligned
        // on the same schema without requiring a separate manual bootstrap step.
        try
        {
            await dbContext.Database.MigrateAsync(cancellationToken);
            await LearningDbSeed.SeedAsync(dbContext, logger, cancellationToken);
        }
        finally
        {
            sessionContextAccessor.TenantId = originalTenantId;
            sessionContextAccessor.BypassRowLevelSecurity = originalBypass;
        }
    }
}