using Blogs.Service.ServiceCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace Blogs.Service.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddScoped<IBlogService, BlogService>()
            .AddScoped<IBlogTagService, BlogTagService>()
            .AddScoped<ICommentsService, CommentsService>()
            .AddScoped<IUserService, UserService>()
            .AddScoped<IJwtService, JwtService>();

        return services;
    }

    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection(AuthOptions.JwtSectionAddress).Get<JwtSettings>();
        services.AddAuthorization();
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
            {
                opt.TokenValidationParameters = new()
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration[AuthOptions.JwtSecretKeySectionAddress]))
                };
                opt.SaveToken = true;
            });

        services.Configure<JwtSettings>(configuration.GetSection(AuthOptions.JwtSectionAddress));
        services.Configure<JwtSettings>(opt =>
            opt.SecretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration[AuthOptions.JwtSecretKeySectionAddress])));

        return services;
    }
}
