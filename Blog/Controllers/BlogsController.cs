using Blogs.Service.Configurations;
using Blogs.Service.Models.RequestModels;
using Blogs.Service.ServiceCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Blogs.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BlogsController(IBlogService blogService) : ControllerBase
    {

        private Guid _userId => new Guid(User.FindFirstValue("UserId"));

        [HttpPost]
        public async Task<ActionResult> CreateBlogAsync(CreateBlogModel createBlogModel, CancellationToken cancellationToken = default)
        {
            await blogService.CreateBlogAsync(createBlogModel, _userId, cancellationToken);

            return new OkResult();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteBlogAsync(Guid blogId, CancellationToken cancellationToken = default) 
        {
            await blogService.DeleteBlogAsync(blogId, cancellationToken);

            return new OkResult();
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> GetAllBlogsAsync([FromQuery] PaginationSettings paginationSettings, CancellationToken cancellationToken = default) 
        {
            return new OkObjectResult(await blogService.GetAllBlogsAsync(paginationSettings, cancellationToken));
        }

        [AllowAnonymous]
        [HttpGet("id")]
        public async Task<ActionResult> GetByIdAsync([FromQuery]Guid id, CancellationToken cancellationToken = default) 
        {
            return new OkObjectResult(await blogService.GetBlogByIdAsync(id, cancellationToken));
        }

        [HttpPatch]
        public async Task<ActionResult> UpdateBlogAsync(UpdateBlogModel blog, CancellationToken cancellationToken) 
        {
            await blogService.UpdateBlogAsync(blog, cancellationToken);
            return new OkResult();
        }

        [HttpPost("add-tags")]
        public async Task<ActionResult> AddTagsToBlogAsync(List<Guid> TagsId, Guid BlogId,  CancellationToken cancellationToken) 
        {
            await blogService.AddTagsToBlogAsync(TagsId, BlogId, _userId, cancellationToken);
            return new OkResult();
        }
    }
}
