using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Neffigy.Xml;

namespace Neffigy
{
    public class XElementAdapter : Element
    {
        private readonly XElement xElement;

        public XElementAdapter(XElement xElement)
        {
            this.xElement = xElement;
        }

        public IEnumerable<Element> Each(string selector)
        {
            var elements = xElement.Css(selector).ToArray();
            foreach (var e in elements)
                yield return new XElementAdapter(e);
        }

        public void Text(object contents)
        {
            xElement.ReplaceNodes(contents);
        }

        public void Attr(string key, string value)
        {
            xElement.SetAttributeValue(key, value);
        }

        public void Remove()
        {
            xElement.Remove();
        }

        public string Attr(string key)
        {
            var attribute = xElement.Attribute(key);
            return attribute == null ? null : attribute.Value;
        }

        public Element Clone()
        {
            return new XElementAdapter(new XElement(xElement));
        }

        public Element AddAfterSelf(Element other)
        {
            xElement.AddAfterSelf(((XElementAdapter)other).xElement);
            return new XElementAdapter(xElement.ElementsAfterSelf().First());
        }

        public override string ToString()
        {
            return xElement.ToString(SaveOptions.DisableFormatting);
        }

        public XElement XElement
        {
            get { return xElement; }
        }

        public void WriteTo(XmlWriter writer)
        {
            xElement.WriteTo(writer);
        }
    }
}