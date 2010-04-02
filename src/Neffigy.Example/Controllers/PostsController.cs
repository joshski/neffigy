using System.Collections.Generic;
using System.Web.Mvc;
using Neffigy.Example.Models;
using Neffigy.Example.Views.Posts;

namespace Neffigy.Example.Controllers
{
    public class PostsController : Controller
    {
        public ActionResult Show(int id)
        {
            var post = new Post
                           {
                               Title = "This is a post, created by PostsController",
                               Body = "And this is the body of the post",
                               Comments = new List<Comment>
                                              {
                                                  new Comment
                                                      {
                                                          Id = 1,
                                                          Title = "This is a comment",
                                                          Summary = "I never liked template languages, or XSL"
                                                      },
                                                  new Comment
                                                      {
                                                          Id = 2,
                                                          Title = "This is another comment",
                                                          Summary = "these are stubs"
                                                      }
                                              },
                           };

            return new ViewResult { View = new ShowPost(post) };
        }
    }
}
