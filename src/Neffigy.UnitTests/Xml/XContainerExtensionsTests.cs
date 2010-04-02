using System.Linq;
using System.Xml.Linq;
using Neffigy.Xml;
using NUnit.Framework;

namespace Neffigy.UnitTests.Xml
{
    [TestFixture]
    public class XContainerExtensionsTests
    {
        [Test]
        public void CssMatchesElementsInHtmlFixture()
        {
            var fixture = XDocument.Parse(ManifestResource.Read("Fixture.html"));
            Assert.AreEqual("pageTitle", fixture.Css("html body[id] div .pageTitle[style=color:red]").First().Attr("class"));
        }

        [Test]
        public void CssMatchesElementsInHtmlFixtureWithRootNamespace()
        {
            var html = ManifestResource.Read("Fixture.html").Replace("<html>", "<html xmlns=\"bob\">");
            var fixture = XDocument.Parse(html);
            Assert.AreEqual("pageTitle", fixture.Css("html body[id] div .pageTitle[style=color:red]").First().Attr("class"));
        }
    }
}