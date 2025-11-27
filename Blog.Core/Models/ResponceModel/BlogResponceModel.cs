using Blogs.Service.Entities;

namespace Blogs.Service.Models.ResponceModel;

public class BlogResponceModel
{
    public Guid BlogId { get; set; }
    public string Name { get; set; }
    public string Text { get; set; }
    public Guid AuthorId { get; set; }

    public List<BlogTag> Tags { get; set; } = new List<BlogTag>();
}
