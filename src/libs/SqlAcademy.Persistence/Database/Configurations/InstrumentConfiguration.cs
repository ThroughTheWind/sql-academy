using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SqlAcademy.Domain.Entities;

namespace SqlAcademy.Persistence.Database.Configurations;

public sealed class InstrumentConfiguration : IEntityTypeConfiguration<Instrument>
{
    public void Configure(EntityTypeBuilder<Instrument> builder)
    {
        builder.ToTable("Instruments");
        builder.HasKey(instrument => instrument.Id);

        builder.Property(instrument => instrument.Symbol)
            .HasMaxLength(24)
            .IsRequired();

        builder.Property(instrument => instrument.Name)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(instrument => instrument.AssetClass)
            .HasConversion<string>()
            .HasMaxLength(32);

        builder.Property(instrument => instrument.TickSize)
            .HasPrecision(18, 4);

        builder.Property(instrument => instrument.LotSize)
            .HasPrecision(18, 4);

        builder.Property(instrument => instrument.CreatedUtc)
            .HasPrecision(3);

        builder.HasIndex(instrument => instrument.Symbol)
            .IsUnique();
    }
}