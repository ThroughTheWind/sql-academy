using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SqlAcademy.Persistence.Database;

namespace SqlAcademy.Persistence.Initialization;

public sealed class LearningDbInitializer(LearningDbContext dbContext, ILogger<LearningDbInitializer> logger)
{
    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        // Applying migrations at startup keeps local runs, container runs, and tests aligned
        // on the same schema without requiring a separate manual bootstrap step.
        await dbContext.Database.MigrateAsync(cancellationToken);
        await LearningDbSeed.SeedAsync(dbContext, logger, cancellationToken);
    }
}