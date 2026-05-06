namespace SqlAcademy.Domain.Entities;

public sealed class Comment
{
    public int Id { get; set; }

    public int PostId { get; set; }

    public int UserId { get; set; }

    public required string Body { get; set; }

    public DateTime CreatedUtc { get; set; }

    public Post Post { get; set; } = null!;

    public User Author { get; set; } = null!;
}