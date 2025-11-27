using Blogs.Service.Entities;

namespace Blogs.Service.RepositoryInterfaces;

public interface IBlogTagRepository: IGenericRepository<BlogTag>
{
    public Task AddTagsToBlogAsync(List<Guid> BlogTagsId, Guid BlogId, CancellationToken cancellationToken);

    public Task<List<BlogTag>> GetBlogTagsToBlogAsync(Guid blogId, CancellationToken cancellationToken);
}
