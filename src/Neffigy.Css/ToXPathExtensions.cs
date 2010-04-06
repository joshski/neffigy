using System.Collections.Generic;
using System.Linq;

namespace Neffigy.Css
{
    public static class ToXPathExtensions
    {
        public static string ToXPath(this IEnumerable<Traversal> traversals, string ns)
        {
            return "." + string.Join("", traversals.Select(t => t.ToXPath(ns)).ToArray()).Replace("/[", "/*[");
        }
    }
}
