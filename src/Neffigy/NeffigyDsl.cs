using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

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

        protected NeffigyDsl() : this(new IdMatchingXElementMerger())
        {
        }

        protected internal override XElement LoadViewAsXElement(XElementLoader loader, string path, Func<string, string> mapPath)
        {
            var masterPath = PathToHtmlFileForType(master.GetType());
            master.RootElement = new XElementAdapter(master.LoadViewAsXElement(loader, masterPath, mapPath));
            var baseElement = base.LoadViewAsXElement(loader, path, mapPath);
            return merger.Merge(master.RootElement.XElement, baseElement);
        }

        protected internal override void InvokeTransform()
        {
            master.InvokeTransform();
            base.InvokeTransform();
        }
    }

    public abstract class NeffigyDsl : Transformation
    {
        protected internal XElementAdapter RootElement { get; set; }

        protected abstract override void Transform();

        protected virtual string PathToHtmlFile
        {
            get {
                return PathToHtmlFileForType(GetType());
            }
        }

        public void Render(TextWriter textWriter, Func<string, string> mapPath, XElementLoader loader)
        {
            RootElement = new XElementAdapter(LoadViewAsXElement(loader, PathToHtmlFile, mapPath));
            InvokeTransform();
            using (var xmlWriter = CreateXmlWriter(textWriter))
            {
                RootElement.WriteTo(xmlWriter);
            }
        }

        protected internal virtual void InvokeTransform()
        {
            Transform(new WrappedSet(RootElement));
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
    }
}