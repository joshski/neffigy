using System.Linq;
using Neffigy.Example.Models;
using Neffigy.Mvc;

namespace Neffigy.Example.Views.Posts
{
    public class ShowPost : NeffigyView<Master>
    {
        private readonly Post post;

        public ShowPost(Post post)
        {
            this.post = post;
        }

        protected override void Transform()
        {
            Text("#main h2", post.Title);
            Text("title", post.Title);
            Text("p.body", post.Body);
            Remove(".comments:gt(0)");
            ReplaceEach(".comment", post.Comments,
                comment => {
                  Text("h3", comment.Title);
                  Text("p", comment.Summary);
                  Attr("a", "href", comment.RelativeUrl());    
                }
            );
            if (post.Comments.Any()) {
                Remove("#no-comments");
            }
        } 
    }
}
