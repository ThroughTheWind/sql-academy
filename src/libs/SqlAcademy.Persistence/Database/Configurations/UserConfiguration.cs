using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SqlAcademy.Domain.Entities;

namespace SqlAcademy.Persistence.Database.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(user => user.Id);

        builder.Property(user => user.UserName)
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(user => user.Email)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(user => user.CreatedUtc)
            .HasPrecision(3);

        builder.HasIndex(user => user.UserName)
            .IsUnique();

        builder.HasIndex(user => user.Email)
            .IsUnique();
    }
}