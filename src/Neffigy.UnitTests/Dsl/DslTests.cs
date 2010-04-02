using System.IO;
using System.Linq;
using System.Xml.Linq;
using Neffigy.Xml;
using NUnit.Framework;

namespace Neffigy.UnitTests.Dsl
{
    public class ExampleTransformer : NeffigyDsl
    {
        protected override void Transform()
        {
            Text("p.body", "q");
            ReplaceEach("p.z", new[] { "a", "b" }, footer =>
                                                   Text("a", "k")
                );
            Remove("#logo");
            Attr(".pageTitle", "class", "somethingElse");
        }
    }

    [TestFixture]
    public class DslTests
    {
        private readonly XElement original = XElement.Parse(ManifestResource.Read("Fixture.html"));
        private XElement trasformed;

        [TestFixtureSetUp]
        public void TransformHtmlFixture()
        {
            var loader = new StubXElementLoader
                             {
                                 { "~/Views/Neffigy/UnitTests/Dsl/ExampleTransformer.html", ManifestResource.Read("Fixture.html") }
                             };
            var writer = new StringWriter();
            new ExampleTransformer().Render(writer, path => path, loader);
            writer.Flush();
            var output = writer.GetStringBuilder().ToString();
            trasformed = XElement.Parse(output);
        }

        [Test]
        public void ReplacesNodeContentsWithText()
        {
            Assert.That(original.Css("p.body").First().ToString(), Is.Not.EqualTo("<p class=\"body\">q</p>"));
            Assert.That(trasformed.Css("p.body").First().ToString(), Is.EqualTo("<p class=\"body\">q</p>"));
        }

        [Test]
        public void IteratesOverEnumerableReplacingElementWithClones()
        {
            var originalTags = original.Css("p.z").Select(e => e.ToString(SaveOptions.DisableFormatting)).ToArray();
            var transformedTags = trasformed.Css("p.z").Select(e => e.ToString(SaveOptions.DisableFormatting)).ToArray();
            Assert.That(originalTags, Is.EquivalentTo(new[] { "<p class=\"z\"><a>y</a></p>" }));
            Assert.That(transformedTags, Is.EquivalentTo(new[] { "<p class=\"z\"><a>k</a></p>", "<p class=\"z\"><a>k</a></p>" }));
        }

        [Test]
        public void RemovesElements()
        {
            Assert.That(!trasformed.Css("#logo").Any());
        }

        [Test]
        public void UpdatesAttributes()
        {
            Assert.That(trasformed.Css(".somethingElse").Count(), Is.EqualTo(1));
        }
    }
}