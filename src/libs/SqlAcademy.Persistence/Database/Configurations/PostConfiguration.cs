using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SqlAcademy.Domain.Entities;

namespace SqlAcademy.Persistence.Database.Configurations;

public sealed class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("Posts");
        builder.HasKey(post => post.Id);

        builder.Property(post => post.Title)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(post => post.Body)
            .HasMaxLength(4_000)
            .IsRequired();

        builder.Property(post => post.CreatedUtc)
            .HasPrecision(3);

        builder.HasOne(post => post.Author)
            .WithMany(user => user.Posts)
            .HasForeignKey(post => post.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(post => new { post.UserId, post.CreatedUtc });
        builder.HasIndex(post => post.Title);
    }
}