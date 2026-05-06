using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SqlAcademy.Domain.Entities;

namespace SqlAcademy.Persistence.Database.Configurations;

public sealed class TradeConfiguration : IEntityTypeConfiguration<Trade>
{
    public void Configure(EntityTypeBuilder<Trade> builder)
    {
        builder.ToTable("Trades");
        builder.HasKey(trade => trade.Id);

        builder.Property(trade => trade.Side)
            .HasConversion<string>()
            .HasMaxLength(16);

        builder.Property(trade => trade.Quantity)
            .HasPrecision(18, 4);

        builder.Property(trade => trade.Price)
            .HasPrecision(18, 4);

        builder.Property(trade => trade.TradedUtc)
            .HasPrecision(3);

        builder.HasOne(trade => trade.User)
            .WithMany(user => user.Trades)
            .HasForeignKey(trade => trade.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(trade => trade.Instrument)
            .WithMany(instrument => instrument.Trades)
            .HasForeignKey(trade => trade.InstrumentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(trade => new { trade.UserId, trade.TradedUtc });
        builder.HasIndex(trade => new { trade.InstrumentId, trade.TradedUtc });
    }
}