using Microsoft.EntityFrameworkCore;
using SqlAcademy.Domain.Entities;

namespace SqlAcademy.Persistence.Database;

public sealed class LearningDbContext(DbContextOptions<LearningDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();

    public DbSet<Post> Posts => Set<Post>();

    public DbSet<Comment> Comments => Set<Comment>();

    public DbSet<Order> Orders => Set<Order>();

    public DbSet<Trade> Trades => Set<Trade>();

    public DbSet<TradeImportBatch> TradeImportBatches => Set<TradeImportBatch>();

    public DbSet<TradeImportBatchRow> TradeImportBatchRows => Set<TradeImportBatchRow>();

    public DbSet<Instrument> Instruments => Set<Instrument>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("academy");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LearningDbContext).Assembly);
    }
}