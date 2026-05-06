using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SqlAcademy.Domain.Entities;

namespace SqlAcademy.Persistence.Database.Configurations;

public sealed class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("Comments");
        builder.HasKey(comment => comment.Id);

        builder.Property(comment => comment.Body)
            .HasMaxLength(2_000)
            .IsRequired();

        builder.Property(comment => comment.CreatedUtc)
            .HasPrecision(3);

        builder.HasOne(comment => comment.Post)
            .WithMany(post => post.Comments)
            .HasForeignKey(comment => comment.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(comment => comment.Author)
            .WithMany(user => user.Comments)
            .HasForeignKey(comment => comment.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(comment => new { comment.PostId, comment.CreatedUtc });
        builder.HasIndex(comment => new { comment.UserId, comment.CreatedUtc });
    }
}