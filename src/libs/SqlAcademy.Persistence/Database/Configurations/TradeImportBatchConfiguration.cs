using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SqlAcademy.Domain.Entities;

namespace SqlAcademy.Persistence.Database.Configurations;

public sealed class TradeImportBatchConfiguration : IEntityTypeConfiguration<TradeImportBatch>
{
    public void Configure(EntityTypeBuilder<TradeImportBatch> builder)
    {
        builder.ToTable("TradeImportBatches");
        builder.HasKey(batch => batch.Id);

        builder.Property(batch => batch.ProcessedUtc)
            .HasPrecision(3);

        builder.Property(batch => batch.Source)
            .HasMaxLength(128);

        builder.Property(batch => batch.CorrelationId)
            .HasMaxLength(128);

        builder.HasMany(batch => batch.Rows)
            .WithOne(row => row.Batch)
            .HasForeignKey(row => row.TradeImportBatchId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(batch => batch.ProcessedUtc);
        builder.HasIndex(batch => batch.CorrelationId);
        builder.HasIndex(batch => new { batch.Source, batch.ProcessedUtc });
    }
}