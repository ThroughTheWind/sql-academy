using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SqlAcademy.Domain.Entities;

namespace SqlAcademy.Persistence.Database.Configurations;

public sealed class TenantOrderConfiguration : IEntityTypeConfiguration<TenantOrder>
{
    public void Configure(EntityTypeBuilder<TenantOrder> builder)
    {
        builder.ToTable("TenantOrders");
        builder.HasKey(order => order.Id);

        builder.Property(order => order.OrderNumber)
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(order => order.Description)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(order => order.TotalAmount)
            .HasPrecision(18, 2);

        builder.Property(order => order.CreatedUtc)
            .HasPrecision(3);

        builder.HasIndex(order => new { order.TenantId, order.OrderNumber })
            .IsUnique();

        builder.HasIndex(order => new { order.TenantId, order.CreatedUtc });
    }
}