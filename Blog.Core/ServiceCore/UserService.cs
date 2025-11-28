using Blogs.Service.Entities;
using Blogs.Service.Helpers;
using Blogs.Service.Models.RequestModels;
using Blogs.Service.RepositoryInterfaces;
using Microsoft.AspNetCore.Identity;

namespace Blogs.Service.ServiceCore;

public interface IUserService 
{
    public Task RegisterUserAsync(CreateUserModel createUserModel, CancellationToken cancellationToken);
    public Task<string> LoginAsync(LoginUserModel loginUserModel, CancellationToken cancellationToken);
    public Task<User> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken);
    public Task DeleteUserAync(Guid userId, CancellationToken cancellationToken);
}

public class UserService(IUserRepository userRepository, IJwtService jwtService) : IUserService
{
    public async Task DeleteUserAync(Guid userId, CancellationToken cancellationToken)
    {
        await userRepository.DeleteAsync(userId, cancellationToken);
    }

    public async Task<User> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(userId, cancellationToken);
        if (user == null) { throw new KeyNotFoundException(); }
        return user;
    }

    public async Task<string> LoginAsync(LoginUserModel loginUserModel, CancellationToken cancellationToken)
    {
        loginUserModel.Password = PasswordHelper.GetPasswordHesh(loginUserModel.Password);
        var user = await userRepository.CheckUserCredentialsAsync(loginUserModel, cancellationToken);

        if (user == null) { throw new KeyNotFoundException("Login or password is incorrect"); }

        return jwtService.GetNewToken(user);
    }

    public async Task RegisterUserAsync(CreateUserModel createUserModel, CancellationToken cancellationToken)
    {
        var user = new User() 
        {
            Login = createUserModel.Login,
            Password = PasswordHelper.GetPasswordHesh(createUserModel.Password),
            Name = createUserModel.Name,
        };

        await userRepository.CreateAsync(user, cancellationToken);
    }
}
