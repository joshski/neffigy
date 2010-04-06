using System.Xml.Linq;
using System.Xml.XPath;

namespace Neffigy
{
    public class IdMatchingXElementMerger : XElementMerger
    {
        public XElement Merge(XElement masterElement, XNode sourceElement)
        {
            var masterElementsWithIds = masterElement.XPathSelectElements("//*[@id]");
            foreach (var masterElementWithId in masterElementsWithIds)
            {
                // ReSharper disable PossibleNullReferenceException
                var childElementWithSameId = sourceElement.XPathSelectElement("//*[@id='" + masterElementWithId.Attribute("id").Value + "']");
                // ReSharper restore PossibleNullReferenceException
                if (childElementWithSameId != null)
                    masterElementWithId.ReplaceWith(childElementWithSameId);
            }
            return masterElement;
        }
    }
}