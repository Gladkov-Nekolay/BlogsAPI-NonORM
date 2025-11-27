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
    public class UserController(IUserService userService) : ControllerBase
    {
        private Guid _userId => new Guid(User.FindFirstValue("UserId"));

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> RegisterUserAsync(CreateUserModel model, CancellationToken cancellationToken = default)
        {
            await userService.RegisterUserAsync(model, cancellationToken);

            return new OkResult();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteUserAsync(CancellationToken cancellationToken = default)
        {
            await userService.DeleteUserAync(_userId, cancellationToken);

            return new OkResult();
        }

        [HttpGet]
        public async Task<ActionResult> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return new OkObjectResult(await userService.GetUserByIdAsync(userId, cancellationToken));
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<string>> LoginAsync(LoginUserModel login, CancellationToken cancellationToken = default) 
        {
            return new OkObjectResult(await userService.LoginAsync(login, cancellationToken));
        }
    }
}
