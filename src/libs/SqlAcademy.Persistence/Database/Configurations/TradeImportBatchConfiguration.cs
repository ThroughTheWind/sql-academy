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

        builder.HasMany(batch => batch.Rows)
            .WithOne(row => row.Batch)
            .HasForeignKey(row => row.TradeImportBatchId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(batch => batch.ProcessedUtc);
    }
}