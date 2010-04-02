using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Neffigy.Css;

namespace Neffigy.Xml
{
    public static class XContainerExtensions
    {
        public static IEnumerable<XElement> Css(this XContainer container, string selector)
        {
            var tokens = new CssTokeniser(new StringReader(selector)).Tokenise();
            var traversals = new CssParser().Parse(tokens);
            var document = container.Document;
            if (document != null && document.Root != null && document.Root.Name.Namespace != null)
            {
                return SelectWithRootNamespace(container, document.Root.Name.NamespaceName, traversals);
            }
            return container.XPathSelectElements(traversals.ToXPath(null)).ToArray();
        }

        static IEnumerable<XElement> SelectWithRootNamespace(XNode container, string namespaceName, IEnumerable<Traversal> traversals)
        {
            var nav = container.CreateNavigator();
            var xPath = traversals.ToXPath("x");
            var manager = new XmlNamespaceManager(nav.NameTable);
            manager.AddNamespace("x", namespaceName);
            return container.XPathSelectElements(xPath, manager);
        }

        public static string Attr(this XElement element, string key)
        {
            var attribute = element.Attribute(key);
            return attribute == null ? null : attribute.Value;
        }
    }
}