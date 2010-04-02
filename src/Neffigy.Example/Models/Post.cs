using System.Collections.Generic;

namespace Neffigy.Example.Models
{
    public class Post
    {
        public IEnumerable<Comment> Comments { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
