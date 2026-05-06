using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SqlAcademy.Domain.Entities;

namespace SqlAcademy.Persistence.Database.Configurations;

public sealed class TradeImportBatchRowConfiguration : IEntityTypeConfiguration<TradeImportBatchRow>
{
    public void Configure(EntityTypeBuilder<TradeImportBatchRow> builder)
    {
        builder.ToTable("TradeImportBatchRows");
        builder.HasKey(row => row.Id);

        builder.Property(row => row.Outcome)
            .HasMaxLength(32);

        builder.Property(row => row.Stage)
            .HasMaxLength(32);

        builder.Property(row => row.Code)
            .HasMaxLength(64);

        builder.Property(row => row.Reason)
            .HasMaxLength(256);

        builder.Property(row => row.UserName)
            .HasMaxLength(64);

        builder.Property(row => row.InstrumentSymbol)
            .HasMaxLength(24);

        builder.Property(row => row.Side)
            .HasMaxLength(16);

        builder.Property(row => row.Quantity)
            .HasPrecision(18, 4);

        builder.Property(row => row.Price)
            .HasPrecision(18, 4);

        builder.Property(row => row.TradedUtc)
            .HasPrecision(3);

        builder.HasOne<Trade>()
            .WithMany()
            .HasForeignKey(row => row.TradeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(row => new { row.TradeImportBatchId, row.RowNumber });
        builder.HasIndex(row => row.TradeId)
            .HasFilter("[TradeId] IS NOT NULL");
    }
}