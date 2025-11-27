using Blogs.Infrastructure.Configurations;
using Blogs.Infrastructure.Extensions;
using Blogs.Service.Entities;
using Blogs.Service.Models.RequestModels;
using Blogs.Service.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Npgsql;
using NpgsqlTypes;
using System.Data.Common;

namespace Blogs.Infrastructure.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory, TableNamesConstants.UsersTableName)
    {
        
    }

    protected override Dictionary<(string FieldName, NpgsqlDbType FieldType), string> MapRequestValuesFromEntity(User entity)
    {
        return new Dictionary<(string FieldName, NpgsqlDbType FieldType), string>
        {
            {("Name", NpgsqlDbType.Text), entity.Name },
            {("Password", NpgsqlDbType.Text), entity.Password},
            {("Login", NpgsqlDbType.Text), entity.Login }
        };
    }

    protected override User MapResultToEntity(DbDataReader reader)
    {
        return new User() 
        {
            Id = (Guid)reader["Id"],
            Name = reader["Name"] as string,
            Password = reader["Password"] as string,
            Login = reader["Login"] as string
        };
    }

    public async Task<User?> CheckUserCredentialsAsync(LoginUserModel loginUserModel, CancellationToken cancellationToken)
    {
        var sql = $"""SELECT * FROM "{TableNamesConstants.UsersTableName}" WHERE "Login" = @Login AND "Password" =@Password""";

        using var connection = await ConnectionFactory.CreateConnectionAsync();
        using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("Login", loginUserModel.Login);
        command.Parameters.AddWithValue("Password", loginUserModel.Password);

        using var reader = await command.ExecuteReaderAsync(cancellationToken);

        if (await reader.ReadAsync(cancellationToken))
        {
            return MapResultToEntity(reader);
        }

        return null;
    }
}