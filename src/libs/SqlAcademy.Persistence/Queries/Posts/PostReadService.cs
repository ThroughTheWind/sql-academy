using Microsoft.EntityFrameworkCore;
using SqlAcademy.Domain.Entities;
using SqlAcademy.Persistence.Database;
using SqlAcademy.Persistence.Diagnostics;
using SqlAcademy.SharedKernel.Pagination;
using SqlAcademy.SharedKernel.Sorting;

namespace SqlAcademy.Persistence.Queries.Posts;

public sealed class PostReadService(LearningDbContext dbContext)
{
    public async Task<PagedResult<PostListItem>> GetPostsAsync(PostQueryRequest request, CancellationToken cancellationToken)
    {
        using var activity = PersistenceDiagnostics.ActivitySource.StartActivity("ef.posts.list");
        activity?.SetTag("posts.page_size", request.NormalizedPageSize);
        activity?.SetTag("posts.sort_by", request.SortBy);

        IQueryable<Post> query = dbContext.Posts.AsNoTracking();

        if (request.AuthorId.HasValue)
        {
            query = query.Where(post => post.UserId == request.AuthorId.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();
            query = query.Where(post =>
                post.Title.Contains(search) ||
                post.Body.Contains(search) ||
                post.Author.UserName.Contains(search));
        }

        query = ApplySorting(query, request.SortBy, request.SortDirection);

        var totalCount = await query.LongCountAsync(cancellationToken);

        var items = await query
            .Skip(request.Skip)
            .Take(request.NormalizedPageSize)
            .Select(post => new PostListItem(
                post.Id,
                post.Title,
                post.Author.UserName,
                post.Comments.Count,
                post.CreatedUtc))
            .ToListAsync(cancellationToken);

        return new PagedResult<PostListItem>(items, request.NormalizedPageNumber, request.NormalizedPageSize, totalCount);
    }

    private static IQueryable<Post> ApplySorting(IQueryable<Post> query, string sortBy, SortDirection sortDirection)
    {
        return (sortBy.Trim().ToLowerInvariant(), sortDirection) switch
        {
            ("title", SortDirection.Asc) => query.OrderBy(post => post.Title).ThenBy(post => post.Id),
            ("title", SortDirection.Desc) => query.OrderByDescending(post => post.Title).ThenByDescending(post => post.Id),
            ("commentcount", SortDirection.Asc) => query.OrderBy(post => post.Comments.Count).ThenBy(post => post.Id),
            ("commentcount", SortDirection.Desc) => query.OrderByDescending(post => post.Comments.Count).ThenByDescending(post => post.Id),
            (_, SortDirection.Asc) => query.OrderBy(post => post.CreatedUtc).ThenBy(post => post.Id),
            _ => query.OrderByDescending(post => post.CreatedUtc).ThenByDescending(post => post.Id),
        };
    }
}