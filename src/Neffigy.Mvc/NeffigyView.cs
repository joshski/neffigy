using System.IO;
using System.Web.Mvc;

namespace Neffigy.Mvc
{
    public abstract class NeffigyView<TMaster> : NeffigyDsl<TMaster>, IView where TMaster : NeffigyDsl, new()
    {
        protected NeffigyView() : this(new OverwriteIdsMerger())
        {
        }

        protected NeffigyView(XElementMerger merger) : base(merger)
        {
        }

        public void Render(ViewContext context, TextWriter writer)
        {
            Render(writer, context.HttpContext.Server.MapPath, new FileSystemXElementLoader());
        }
    }

    public abstract class NeffigyView : NeffigyDsl, IView
    {
        public void Render(ViewContext context, TextWriter textWriter)
        {
            Render(textWriter, context.HttpContext.Server.MapPath, new FileSystemXElementLoader());
        }
    }
}