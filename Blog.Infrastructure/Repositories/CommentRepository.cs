using Blogs.Infrastructure.Configurations;
using Blogs.Infrastructure.Extensions;
using Blogs.Service.Configurations;
using Blogs.Service.Entities;
using Blogs.Service.RepositoryInterfaces;
using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Npgsql;
using NpgsqlTypes;
using System.Data.Common;

namespace Blogs.Infrastructure.Repositories;

public class CommentRepository : GenericRepository<Comment>, ICommentRepository
{
    public CommentRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory, TableNamesConstants.CommentsTableName)
    {
    }

    protected override Dictionary<(string FieldName, NpgsqlDbType FieldType), string> MapRequestValuesFromEntity(Comment entity)
    {
        return new Dictionary<(string FieldName, NpgsqlDbType FieldType), string>
        {
            {("Content", NpgsqlDbType.Text),  entity.Content },
            {("BlogId", NpgsqlDbType.Uuid), entity.BlogId.ToString()},
            {("AuthorId", NpgsqlDbType.Uuid), entity.AuthorId.ToString()}
        };
    }

    protected override Comment MapResultToEntity(DbDataReader reader)
    {
        return new Comment()
        {
            Id = (Guid)reader["Id"],
            Content = reader["Content"] as string,
            BlogId = (Guid)reader["BlogId"],
            AuthorId = (Guid)reader["AuthorId"]
        };
    }

    public async Task<IEnumerable<Comment>> GetCommentsToBlogAsync(Guid blogId, PaginationSettings paginationSettings, CancellationToken cancellationToken)
    {
        var sql = $"""
            SELECT * FROM "{TableNamesConstants.CommentsTableName}"
            WHERE "BlogId" = @BlogId
            LIMIT {paginationSettings.PageSize} 
            OFFSET {paginationSettings.PageSize * (paginationSettings.PageNumber - 1)}
            """;

        using var connection = await ConnectionFactory.CreateConnectionAsync();
        using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("BlogId", blogId);

        using var reader = await command.ExecuteReaderAsync(cancellationToken);
        var results = new List<Comment>();

        while (await reader.ReadAsync(cancellationToken))
        {
            results.Add(MapResultToEntity(reader));
        }
        return results;
    }
}
