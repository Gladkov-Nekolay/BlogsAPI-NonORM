using Blogs.Service.Configurations;
using Blogs.Service.Entities;
using Blogs.Service.ServiceCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blogs.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BlogTagsController(IBlogTagService blogTagService) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> CreateBlogTagAsync(BlogTag blogTag, CancellationToken cancellationToken = default)
        {
            await blogTagService.CreateBlogTagAsync(blogTag, cancellationToken);

            return new OkResult();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteBlogTagAsync(Guid blogId, CancellationToken cancellationToken = default)
        {
            await blogTagService.DeleteBlogTagAsync(blogId, cancellationToken);

            return new OkResult();
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> GetAllBlogTagsAsync([FromQuery] PaginationSettings paginationSettings, CancellationToken cancellationToken = default)
        {
            return new OkObjectResult(await blogTagService.GetAllBlogTagsAsync(paginationSettings, cancellationToken));
        }

        [AllowAnonymous]
        [HttpGet("id")]
        public async Task<ActionResult> GetByIdAsync([FromQuery] Guid id, CancellationToken cancellationToken = default)
        {
            return new OkObjectResult(await blogTagService.GetBlogTagAsync(id, cancellationToken));
        }

        [HttpPatch]
        public async Task<ActionResult> UpdateBlogTagAsync(BlogTag blogTag, CancellationToken cancellationToken)
        {
            await blogTagService.UpdateBlogTagAsync(blogTag, cancellationToken);
            return new OkResult();
        }
    }
}
