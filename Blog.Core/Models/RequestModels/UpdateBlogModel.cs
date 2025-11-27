using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Service.Models.RequestModels;

public class UpdateBlogModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Text { get; set; }
}
