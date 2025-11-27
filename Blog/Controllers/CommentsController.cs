using Blogs.Service.Configurations;
using Blogs.Service.Entities;
using Blogs.Service.ServiceCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Blogs.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentsController(ICommentsService commentsService) : ControllerBase
    {
        private Guid _userId => new Guid(User.FindFirstValue("UserId"));
        [HttpPost]
        public async Task<ActionResult> CreateCommentAsync(Comment commnet, CancellationToken cancellationToken = default)
        {
            await commentsService.CreateCommentAsync(commnet, _userId, cancellationToken);

            return new OkResult();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteCommentAsync(Guid CommentId, CancellationToken cancellationToken = default)
        {
            await commentsService.DeleteCommentAsync(CommentId, cancellationToken);

            return new OkResult();
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> GetCommentsToBlogAsync([FromQuery] PaginationSettings paginationSettings, Guid blogId, CancellationToken cancellationToken = default)
        {
            return new OkObjectResult(await commentsService.GetCommentsToBlogAsync(paginationSettings, blogId, cancellationToken));
        }
    }
}
