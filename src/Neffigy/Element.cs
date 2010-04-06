using System.Collections.Generic;

namespace Neffigy
{
    public interface Element
    {
        IEnumerable<Element> Each(string selector);
        void Text(object contents);
        void Attr(string key, string value);
        void Remove();
        string Attr(string key);
        Element Clone();
        Element AddAfterSelf(Element element);
    }
}