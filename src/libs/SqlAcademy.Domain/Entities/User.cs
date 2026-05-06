namespace SqlAcademy.Domain.Entities;

public sealed class User
{
    public int Id { get; set; }

    public required string UserName { get; set; }

    public required string Email { get; set; }

    public DateTime CreatedUtc { get; set; }

    public List<Post> Posts { get; } = [];

    public List<Comment> Comments { get; } = [];

    public List<Order> Orders { get; } = [];

    public List<Trade> Trades { get; } = [];
}