namespace Blogs.Service.Models.RequestModels;

public class CreateBlogModel
{
    public string Name { get; set; }
    public string Text { get; set; } = string.Empty;
}
