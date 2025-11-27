using Blogs.Service.Configurations;
using Blogs.Service.Entities;
using Blogs.Service.Models.RequestModels;
using Blogs.Service.Models.ResponceModel;
using Blogs.Service.RepositoryInterfaces;

namespace Blogs.Service.ServiceCore;

public interface IBlogService 
{
    public Task AddTagsToBlogAsync(List<Guid> TagsId, Guid BlogId, Guid userId, CancellationToken cancellationToken);
    public Task CreateBlogAsync(CreateBlogModel model, Guid AuthorId, CancellationToken cancellationToken);
    public Task DeleteBlogAsync(Guid BlogId, CancellationToken cancellationToken);
    public Task UpdateBlogAsync(UpdateBlogModel updateBlogModel, CancellationToken cancelToken);
    public Task<BlogResponceModel> GetBlogByIdAsync(Guid BlogId, CancellationToken cancellationToken);
    public Task<List<BlogShortResponceModel>> GetAllBlogsAsync(PaginationSettings paginationSettings, CancellationToken cancellationToken);
}

public class BlogService(IBlogRepository blogRepository, IBlogTagRepository blogTagRepository) : IBlogService
{
    public async Task AddTagsToBlogAsync(List<Guid> TagsId, Guid BlogId, Guid userId, CancellationToken cancellationToken)
    {
        var blog = await blogRepository.GetByIdAsync(BlogId, cancellationToken);

        if (blog == null) 
        { 
            throw new KeyNotFoundException(); 
        }

        if (blog.AuthorId != userId) 
        { 
            throw new MemberAccessException(); 
        }

        await blogTagRepository.AddTagsToBlogAsync(TagsId, BlogId, cancellationToken);
    }

    public async Task CreateBlogAsync(CreateBlogModel model, Guid AuthorId, CancellationToken cancellationToken)
    {
        var blog = new Blog()
        {
            Name = model.Name,
            Text = model.Text,
            AuthorId = AuthorId,
        };

        await blogRepository.CreateAsync(blog, cancellationToken);
    }

    public Task DeleteBlogAsync(Guid BlogId, CancellationToken cancellationToken)
    {
        return blogRepository.DeleteAsync(BlogId, cancellationToken);
    }

    public async Task<List<BlogShortResponceModel>> GetAllBlogsAsync(PaginationSettings paginationSettings, CancellationToken cancellationToken)
    {
        var blogs = await blogRepository.GetAllAsync(paginationSettings, cancellationToken);
        return blogs.Select(blog => new BlogShortResponceModel() 
        {
            BlogId = blog.Id,
            Name=blog.Name,
            AuthorId = blog.AuthorId,
        }).ToList();
    }

    public async Task<BlogResponceModel> GetBlogByIdAsync(Guid BlogId, CancellationToken cancellationToken)
    {
        var blog = await blogRepository.GetByIdAsync(BlogId, cancellationToken);

        if (blog == null) 
        { 
            throw new KeyNotFoundException(); 
        }

        var blogTags = await blogTagRepository.GetBlogTagsToBlogAsync(BlogId, cancellationToken);
        return new BlogResponceModel()
        {
            BlogId = blog.Id,
            Name = blog.Name,
            AuthorId = blog.AuthorId,
            Text = blog.Text,
            Tags = blogTags,
        };
    }

    public async Task UpdateBlogAsync(UpdateBlogModel updateBlogModel, CancellationToken cancellationToken)
    {
        var blog = await blogRepository.GetByIdAsync(updateBlogModel.Id, cancellationToken);

        if(blog == null)
        {
            throw new KeyNotFoundException();
        }

        blog.Id = updateBlogModel.Id;
        blog.Name = updateBlogModel.Name;
        blog.Text = updateBlogModel.Text;
        blog.AuthorId = blog.AuthorId;

        await blogRepository.UpdateAsync(blog, cancellationToken);
    }
}
