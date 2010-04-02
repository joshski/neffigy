using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Neffigy.Xml;

namespace Neffigy
{
    public abstract class NeffigyDsl<TMaster> : NeffigyDsl where TMaster : NeffigyDsl, new()
    {
        private readonly TMaster master = new TMaster();
        private readonly XElementMerger merger;

        protected NeffigyDsl(XElementMerger merger)
        {
            this.merger = merger;
        }

        protected NeffigyDsl() : this(new OverwriteIdsMerger())
        {
        }

        protected internal override XElement LoadViewAsXElement(XElementLoader loader, string path, Func<string, string> mapPath)
        {
            var masterPath = PathToHtmlFileForType(master.GetType());
            master.SourceElement = master.LoadViewAsXElement(loader, masterPath, mapPath);
            var baseElement = base.LoadViewAsXElement(loader, path, mapPath);
            return merger.Merge(master.SourceElement, baseElement);
        }

        protected internal override void InvokeTransform()
        {
            master.InvokeTransform();
            base.InvokeTransform();
        }
    }

    public abstract class NeffigyDsl
    {
        protected abstract void Transform();

        protected internal XElement SourceElement { get; set; }

        protected virtual string PathToHtmlFile
        {
            get {
                return PathToHtmlFileForType(GetType());
            }
        }

        public void Render(TextWriter textWriter, Func<string, string> mapPath, XElementLoader loader)
        {
            using (var xmlWriter = CreateXmlWriter(textWriter))
            {
                SourceElement = LoadViewAsXElement(loader, PathToHtmlFile, mapPath);
                InvokeTransform();
                SourceElement.WriteTo(xmlWriter);
            }
        }

        protected internal virtual void InvokeTransform()
        {
            Transform();
        }

        protected internal virtual XElement LoadViewAsXElement(XElementLoader loader, string path, Func<string, string> mapPath)
        {
            return loader.Load(path, mapPath);
        }

        protected virtual string PathToHtmlFileForType(Type type)
        {
            return "~/Views/" + string.Join("/", type.Namespace.Split('.').Reverse().TakeWhile(p => p != "Views").Reverse().ToArray()) + "/" + type.Name + ".html";
        }

        static XmlWriter CreateXmlWriter(TextWriter writer)
        {
            return XmlWriter.Create(writer);
        }

        protected void Remove(string selector)
        {
            var elements = Css(selector);
            foreach (var element in elements)
            {
                element.Remove();
            }
        }

        protected void Attr(string selector, string key, object value)
        {
            var elements = Css(selector);
            foreach (var element in elements)
            {
                element.SetAttributeValue(key, value);
            }
        }

        protected void ReplaceEach<T>(string selector, IEnumerable<T> enumerable, Action<T> action)
        {
            var originalSource = SourceElement;
            var selectedElements = SourceElement.Css(selector);
            foreach (var selectedElement in selectedElements)
            {
                ReplaceSelectedElement(selectedElement, enumerable, action);
            }
            SourceElement = originalSource;
        }

        private void ReplaceSelectedElement<T>(XElement selectedElement, IEnumerable<T> enumerable, Action<T> action)
        {
            var element = selectedElement;
            foreach (var item in enumerable)
            {
                SourceElement = new XElement(selectedElement);
                action(item);
                element.AddAfterSelf("\n", SourceElement);
                element = SourceElement;
            }
            selectedElement.Remove();
        }

        protected void Text(string selector, object text)
        {
            var elements = SourceElement.Css(selector);
            foreach (var element in elements)
            {
                element.ReplaceNodes(text);
            }
        }

        protected IEnumerable<XElement> Css(string selector)
        {
            return SourceElement.Css(selector).ToArray();
        }
    }
}