using Blogs.Infrastructure.Configurations;
using Blogs.Infrastructure.Repositories;
using Blogs.Service.RepositoryInterfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blogs.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddScoped<IBlogRepository, BlogRepository>()
            .AddScoped<IBlogTagRepository, BlogTagRepository>()
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<ICommentRepository, CommentRepository>()
            
            .AddScoped<IDbConnectionFactory, ConnectionFactory>();

        services.Configure<DbConnectionConfiguration>(x => x.DbConnectionString = configuration.GetSection(DbConnectionConfiguration.ConnectionStringsSection)
                                                                                               .GetSection(DbConnectionConfiguration.DbConnectionStringSection).Value);

        return services;
    }
}
