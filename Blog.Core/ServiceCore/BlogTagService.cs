using Blogs.Service.Configurations;
using Blogs.Service.Entities;
using Blogs.Service.RepositoryInterfaces;

namespace Blogs.Service.ServiceCore;

public interface IBlogTagService 
{
    public Task CreateBlogTagAsync(BlogTag blogTag, CancellationToken cancellationToken);
    public Task DeleteBlogTagAsync(Guid blogTagId, CancellationToken cancellationToken);
    public Task UpdateBlogTagAsync(BlogTag blogTag, CancellationToken cancellationToken);
    public Task<BlogTag> GetBlogTagAsync(Guid Id, CancellationToken cancellationToken);
    public Task<IEnumerable<BlogTag>> GetAllBlogTagsAsync (PaginationSettings paginationSettings, CancellationToken cancellationToken);
}

public class BlogTagService(IBlogTagRepository blogTagRepository) : IBlogTagService
{
    public async Task CreateBlogTagAsync(BlogTag blogTag, CancellationToken cancellationToken)
    {
        if (blogTag is null) { throw new ArgumentException("Blog tag is empty"); }
        await blogTagRepository.CreateAsync(blogTag, cancellationToken);
    }

    public async Task DeleteBlogTagAsync(Guid blogTagId, CancellationToken cancellationToken)
    {
        await blogTagRepository.DeleteAsync(blogTagId, cancellationToken);
    }

    public async Task<IEnumerable<BlogTag>> GetAllBlogTagsAsync(PaginationSettings paginationSettings, CancellationToken cancellationToken)
    {
        return await blogTagRepository.GetAllAsync(paginationSettings, cancellationToken);
    }

    public async Task<BlogTag> GetBlogTagAsync(Guid Id, CancellationToken cancellationToken)
    {
        return await blogTagRepository.GetByIdAsync(Id, cancellationToken);
    }

    public async Task UpdateBlogTagAsync(BlogTag blogTag, CancellationToken cancellationToken)
    {
        await blogTagRepository.UpdateAsync(blogTag, cancellationToken);
    }
}
