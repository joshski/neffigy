using System.IO;
using System.Linq;
using System.Xml.Linq;
using Neffigy.Xml;
using NUnit.Framework;

namespace Neffigy.UnitTests.Dsl
{
    [TestFixture]
    public class DslWithMasterTests
    {
        private readonly XElement master = XElement.Parse(ManifestResource.Read("Master.html"));
        private readonly XElement child = XElement.Parse(ManifestResource.Read("Fixture.html"));

        private XElement merged;

        [TestFixtureSetUp]
        public void TransformHtmlFixture()
        {
            var loader = new StubXElementLoader
                             {
                                 { "~/Views/Neffigy/UnitTests/Dsl/ExampleTransformerWithMaster.html",
                                     ManifestResource.Read("Fixture.html") },
                                 { "~/Views/Neffigy/UnitTests/Dsl/ExampleMasterTransformer.html",
                                     ManifestResource.Read("Master.html") }
                             };
            var writer = new StringWriter();
            new ExampleTransformerWithMaster().Render(writer, path => path, loader);
            writer.Flush();
            var output = writer.GetStringBuilder().ToString();
            merged = XElement.Parse(output);
        }

        [Test]
        public void OverwritesMasterElementsWhereChildElementExistsWithSameId()
        {
            Assert.That(master.Css("p#heading").Any(), Is.True);
            Assert.That(child.Css("div#heading").Any(), Is.True);

            Assert.That(merged.Css("p#heading").Any(), Is.False);
            Assert.That(merged.Css("div#heading").Any(), Is.True);
        }

        [Test]
        public void DoesNotMergeElementsOnlyPresentInChild()
        {
            Assert.That(master.Css("p.body").Any(), Is.False);
            Assert.That(child.Css("p.body").Any(), Is.True);

            Assert.That(merged.Css("p.body").Any(), Is.False);
        }
    }

    public class ExampleTransformerWithMaster : NeffigyDsl<ExampleMasterTransformer>
    {
        protected override void Transform()
        {
        }
    }

    public class ExampleMasterTransformer : NeffigyDsl
    {
        protected override void Transform()
        {
        }
    }
}