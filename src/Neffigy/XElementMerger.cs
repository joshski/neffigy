using System.Xml.Linq;

namespace Neffigy
{
    public interface XElementMerger
    {
        XElement Merge(XElement masterElement, XNode sourceElement);
    }
}