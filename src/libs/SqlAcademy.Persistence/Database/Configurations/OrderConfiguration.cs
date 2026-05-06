using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SqlAcademy.Domain.Entities;

namespace SqlAcademy.Persistence.Database.Configurations;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");
        builder.HasKey(order => order.Id);

        builder.Property(order => order.OrderNumber)
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(order => order.Status)
            .HasConversion<string>()
            .HasMaxLength(32);

        builder.Property(order => order.TotalAmount)
            .HasPrecision(18, 2);

        builder.Property(order => order.CreatedUtc)
            .HasPrecision(3);

        builder.Property(order => order.UpdatedUtc)
            .HasPrecision(3);

        builder.Property(order => order.RowVersion)
            .IsRowVersion();

        builder.HasOne(order => order.User)
            .WithMany(user => user.Orders)
            .HasForeignKey(order => order.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(order => order.OrderNumber)
            .IsUnique();

        builder.HasIndex(order => new { order.UserId, order.CreatedUtc });
    }
}