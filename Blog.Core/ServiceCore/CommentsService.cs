using Blogs.Service.Configurations;
using Blogs.Service.Entities;
using Blogs.Service.RepositoryInterfaces;

namespace Blogs.Service.ServiceCore;

public interface ICommentsService 
{
    public Task<IEnumerable<Comment>> GetCommentsToBlogAsync(PaginationSettings paginationSettings, Guid BlogId, CancellationToken cancellationToken);
    public Task CreateCommentAsync(Comment comment, Guid userId, CancellationToken cancellationToken);
    public Task DeleteCommentAsync(Guid CommentId, CancellationToken cancellationToken);

}
public class CommentsService(ICommentRepository commentRepository) : ICommentsService
{
    public async Task CreateCommentAsync(Comment comment, Guid userId, CancellationToken cancellationToken)
    {
        comment.AuthorId = userId;
        await commentRepository.CreateAsync(comment, cancellationToken);
    }

    public async Task DeleteCommentAsync(Guid CommentId, CancellationToken cancellationToken)
    {
        await commentRepository.DeleteAsync(CommentId, cancellationToken);
    }

    public async Task<IEnumerable<Comment>> GetCommentsToBlogAsync(PaginationSettings paginationSettings, Guid BlogId, CancellationToken cancellationToken)
    {
        return await commentRepository.GetCommentsToBlogAsync(BlogId, paginationSettings, cancellationToken);
    }
}
