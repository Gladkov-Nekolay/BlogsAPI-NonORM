using Blogs.Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Blogs.Infrastructure.Extensions;

public interface IDbConnectionFactory
{
    Task<NpgsqlConnection> CreateConnectionAsync();
    NpgsqlConnection CreateConnection();
}

public class ConnectionFactory : IDbConnectionFactory
{
    private readonly string? _connectionString;

    public ConnectionFactory(IOptions<DbConnectionConfiguration> configuration)
    {
        _connectionString = configuration.Value.DbConnectionString;
    }

    public async Task<NpgsqlConnection> CreateConnectionAsync()
    {
        var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        return connection;
    }

    public NpgsqlConnection CreateConnection()
    {
        var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        return connection;
    }
}
