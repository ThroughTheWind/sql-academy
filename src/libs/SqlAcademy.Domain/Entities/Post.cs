namespace SqlAcademy.Domain.Entities;

public sealed class Post
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public required string Title { get; set; }

    public required string Body { get; set; }

    public DateTime CreatedUtc { get; set; }

    public User Author { get; set; } = null!;

    public List<Comment> Comments { get; } = [];
}