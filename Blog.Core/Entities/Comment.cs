namespace Blogs.Service.Entities;

public class Comment : BasicEntity
{
    public string Content { get; set; }
    public Guid BlogId { get; set; }
    public Guid AuthorId { get; set; }
}
