using System.Xml.Linq;
using NUnit.Framework;

namespace Neffigy.UnitTests
{
    [TestFixture]
    public class XElementMergerTests
    {
        [Test]
        public void MergesXElementsById()
        {
            var merger = new OverwriteIdsMerger();
            var master = XElement.Parse("<a><b id=\"x\">foo</b><c /></a>");
            var child = XElement.Parse("<a><k id=\"x\">foo</k></a>");
            var merged = merger.Merge(master, child);
            Assert.AreEqual("<a><k id=\"x\">foo</k><c /></a>", merged.ToString(SaveOptions.DisableFormatting));
        }
    }
}