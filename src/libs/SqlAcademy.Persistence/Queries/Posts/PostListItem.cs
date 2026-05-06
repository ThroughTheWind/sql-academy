namespace SqlAcademy.Persistence.Queries.Posts;

public sealed record PostListItem(int Id, string Title, string AuthorUserName, int CommentCount, DateTime CreatedUtc);