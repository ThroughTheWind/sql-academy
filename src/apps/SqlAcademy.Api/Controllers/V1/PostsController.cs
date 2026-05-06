using Microsoft.AspNetCore.Mvc;
using SqlAcademy.Persistence.Queries.Posts;
using SqlAcademy.SharedKernel.Pagination;

namespace SqlAcademy.Api.Controllers.V1;

[ApiController]
[Route("api/v1/posts")]
public sealed class PostsController(PostReadService postReadService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<PostListItem>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<PostListItem>>> GetPosts(
        [FromQuery] PostQueryRequest request,
        CancellationToken cancellationToken)
    {
        var result = await postReadService.GetPostsAsync(request, cancellationToken);
        return Ok(result);
    }
}