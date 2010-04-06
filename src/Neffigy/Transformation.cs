using System;
using System.Collections.Generic;
using System.Linq;

namespace Neffigy
{
    public abstract class Transformation
    {
        private WrappedSet currentSet;

        public void Transform(WrappedSet set)
        {
            currentSet = set;
            Transform();
        }

        protected abstract void Transform();

        public WrappedSet Each(string selector)
        {
            return currentSet.Each(selector);
        }

        public WrappedSet Attr(string selector, string key, string value)
        {
            return Each(selector).Attr(key, value);
        }

        public WrappedSet Text(string selector, string value)
        {
            return Each(selector).Text(value);
        }

        public WrappedSet ReplaceAll<T>(string selector, IEnumerable<T> items, Action<T> eachItemAction)
        {
            var originalSet = currentSet;
            var each = Each(selector);
            each.Skip(1).RemoveAll();
            var first = currentSet = Each(selector).Single();
            var last = first;
            var clones = new List<WrappedSet>();
            foreach (var item in items)
            {
                clones.Add(currentSet = last = last.AppendAfterSelf(first.Clone()));
                eachItemAction(item);
            }
            first.Remove();
            currentSet = originalSet;
            return new WrappedSet(clones);
        }

        public void Remove(string selector)
        {
            Each(selector).Remove();
        }
    }
}
