using System.Collections.Generic;

namespace Neffigy
{
    public static class WrappedSetExtensions
    {
        public static void RemoveAll(this IEnumerable<WrappedSet> sets)
        {
            foreach (var set in sets)
            {
                set.Remove();
            }
        }
    }
}