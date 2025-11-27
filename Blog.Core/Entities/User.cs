namespace Blogs.Service.Entities;

public class User : BasicEntity
{
    public string? Name { get; set; }
    public string Password { get; set; }
    public string Login { get; set; }
}
