using Neffigy.Example.Models;

namespace Neffigy.Example
{
    public static class Urls
    {
        public static string RelativeUrl(this Comment comment)
        {
            return "/comments/" + comment.Id;
        }
    }
}
