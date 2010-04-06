using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Neffigy
{
    public class WrappedSet : IEnumerable<WrappedSet>
    {
        private readonly IEnumerable<Element> elements;

        public WrappedSet(IEnumerable<Element> elements)
        {
            this.elements = elements;
        }

        public WrappedSet(IEnumerable<WrappedSet> wrappedSets)
            : this(wrappedSets.SelectMany(w => w.elements))
        {
        }

        public WrappedSet(Element element)
            : this(new[] { element })
        {
        }

        public IEnumerator<WrappedSet> GetEnumerator()
        {
            return elements.Select(t => new WrappedSet(t)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public WrappedSet Each(string selector)
        {
            var sets = elements.SelectMany(s => s.Each(selector)).ToArray();
            return new WrappedSet(sets);
        }

        public WrappedSet Text(string text)
        {
            foreach (var element in elements)
            {
                element.Text(text);
            }
            return this;
        }

        public WrappedSet First(string selector)
        {
            return Each(selector).First();
        }

        public WrappedSet Last(string selector)
        {
            return Each(selector).Last();
        }

        public WrappedSet Attr(string key, string value)
        {
            foreach (var element in elements)
            {
                element.Attr(key, value);
            }
            return this;
        }

        public string Attr(string key)
        {
            return elements.First().Attr(key);
        }

        public void Remove()
        {
            foreach (var element in elements)
            {
                element.Remove();
            }
        }

        public WrappedSet Clone()
        {
            return new WrappedSet(elements.Select(e => e.Clone()));
        }

        public WrappedSet AppendAfterSelf(WrappedSet set)
        {
            var appended = new List<Element>();
            foreach (var element in elements)
            {
                foreach (var toAppend in set)
                {
                    foreach (var e in toAppend.elements)
                    {
                        appended.Add(element.AddAfterSelf(e));
                    }
                }
            }
            return new WrappedSet(appended);
        }


        public override string ToString()
        {
            return "WrappedSet(" + string.Join(", ", elements.Select(e => e.ToString()).ToArray()) + ')';
        }
    }
}