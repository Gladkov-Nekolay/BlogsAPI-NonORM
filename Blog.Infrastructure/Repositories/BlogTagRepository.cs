using Blogs.Infrastructure.Configurations;
using Blogs.Infrastructure.Extensions;
using Blogs.Service.Entities;
using Blogs.Service.RepositoryInterfaces;
using Microsoft.AspNetCore.Connections;
using Npgsql;
using NpgsqlTypes;
using System.Collections.Generic;
using System.Data.Common;

namespace Blogs.Infrastructure.Repositories;

public class BlogTagRepository : GenericRepository<BlogTag>, IBlogTagRepository
{
    public BlogTagRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory, TableNamesConstants.BlogTagsTableName)
    {
    }

    public async Task AddTagsToBlogAsync(List<Guid> BlogTagsId, Guid BlogId, CancellationToken cancellationToken)
    {
        var insertValues = BlogTagsId.Select(BlogTagId => $"('{BlogId}', '{BlogTagId}')");

        var columnsValues = string.Join(", ", insertValues);

        var sql = $"""INSERT INTO "{TableNamesConstants.BlogsToTagsTableName}" ("BlogId", "BlogTagId") VALUES {columnsValues}""";

        using var connection = await ConnectionFactory.CreateConnectionAsync();
        using var command = new NpgsqlCommand(sql, connection);

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task<List<BlogTag>> GetBlogTagsToBlogAsync(Guid blogId, CancellationToken cancellationToken)
    {
        var sql = $"""
            SELECT "Id", "Name"
            FROM "{TableNamesConstants.BlogTagsTableName}" t
            JOIN "{TableNamesConstants.BlogsToTagsTableName}" bt ON t."Id" = bt."BlogTagId"
            WHERE bt."BlogId" = @BlogId;
            """;

        using var connection = await ConnectionFactory.CreateConnectionAsync();
        using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("BlogId", blogId);

        using var reader = await command.ExecuteReaderAsync(cancellationToken);
        var results = new List<BlogTag>();

        while (await reader.ReadAsync(cancellationToken))
        {
            results.Add(MapResultToEntity(reader));
        }
        return results;
    }

    protected override Dictionary<(string FieldName, NpgsqlDbType FieldType), string> MapRequestValuesFromEntity(BlogTag entity)
    {
        return new Dictionary<(string FieldName, NpgsqlDbType FieldType), string>
        {
            {("Name", NpgsqlDbType.Text), entity.Name }
        };
    }

    protected override BlogTag MapResultToEntity(DbDataReader reader)
    {
        return new BlogTag()
        {
            Id = (Guid)reader["Id"],
            Name = reader["Name"] as string,
        };
    }
}
