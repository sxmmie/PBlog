using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blogger.Models
{
    public class Post
    {
        public int Id { get; set; }

        public string Title { get; set; } = "";
        public string Body { get; set; } = "";
        public string Image { get; set; } = "";     // Represents the name of file since we know where imges are stored -> blog.

        public DateTime Created { get; set; } = DateTime.Now;
    }
}
