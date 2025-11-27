namespace Blogs.Service.Models.RequestModels;

public class CreateUserModel
{
    public string Login { get; set; }
    public string Password { get; set; }
    public string? Name { get; set; }
}
