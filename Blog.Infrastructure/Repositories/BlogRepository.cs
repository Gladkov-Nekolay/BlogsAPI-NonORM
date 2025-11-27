using Blogs.Infrastructure.Configurations;
using Blogs.Infrastructure.Extensions;
using Blogs.Service.Entities;
using Blogs.Service.RepositoryInterfaces;
using NpgsqlTypes;
using System.Data.Common;

namespace Blogs.Infrastructure.Repositories;

public class BlogRepository : GenericRepository<Blog>, IBlogRepository
{
    public BlogRepository(IDbConnectionFactory connectionFactory): base(connectionFactory, TableNamesConstants.BlogsTableName)
    {
    }

    protected override Dictionary<(string FieldName, NpgsqlDbType FieldType), string> MapRequestValuesFromEntity(Blog entity)
    {
        return new Dictionary<(string FieldName, NpgsqlDbType FieldType), string>
        {
            {("Name", NpgsqlDbType.Text), entity.Name },
            {("Text", NpgsqlDbType.Text), entity.Text},
            {("AuthorId", NpgsqlDbType.Uuid), entity.AuthorId.ToString() }
        };
    }

    protected override Blog MapResultToEntity(DbDataReader reader)
    {
        return new Blog
        {
            Id = (Guid)reader["Id"],
            Name = reader["Name"] as string,
            Text = reader["Text"] as string,
            AuthorId = (Guid)reader["AuthorId"]
        };
    }
}
