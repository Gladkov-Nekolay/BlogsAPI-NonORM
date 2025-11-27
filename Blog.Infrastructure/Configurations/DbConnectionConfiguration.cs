namespace Blogs.Infrastructure.Configurations;

public class DbConnectionConfiguration
{
    public const string ConnectionStringsSection = "ConnectionStrings";
    public const string DbConnectionStringSection = "DbConnectionString";

    public string? DbConnectionString { get; set; }
}
