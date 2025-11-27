using Blogs.Service.Entities;
using Blogs.Service.Models.RequestModels;

namespace Blogs.Service.RepositoryInterfaces;

public interface IUserRepository: IGenericRepository<User>
{
    public Task<User?> CheckUserCredentialsAsync(LoginUserModel loginUserModel, CancellationToken cancellationToken);
}
