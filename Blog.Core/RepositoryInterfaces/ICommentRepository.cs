using Blogs.Service.Configurations;
using Blogs.Service.Entities;

namespace Blogs.Service.RepositoryInterfaces;

public interface ICommentRepository: IGenericRepository<Comment>
{
    public Task<IEnumerable<Comment>> GetCommentsToBlogAsync(Guid blogId, PaginationSettings paginationSettings, CancellationToken cancellationToken);
}
