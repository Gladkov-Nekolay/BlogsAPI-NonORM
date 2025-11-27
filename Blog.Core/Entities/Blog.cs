using Blogs.Service.Models.RequestModels;

namespace Blogs.Service.Entities;

public class Blog : BasicEntity
{
    public string Name { get; set; }
    public string Text { get; set; }
    public Guid AuthorId { get; set; }
}
