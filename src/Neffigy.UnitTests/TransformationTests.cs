using System.Xml.Linq;
using NUnit.Framework;

namespace Neffigy.UnitTests
{
    [TestFixture]
    public class TransformationTests
    {
        [Test]
        public void Transforms()
        {
            var fixture = XElement.Parse(@"<z><p></p><p></p><q><h></h></q><q><h></h></q></z>");
            var element = new XElementAdapter(fixture);
            var set = new WrappedSet(element);
            new ExampleTransformation().Transform(set);
            Assert.AreEqual("B", set.First("p").Attr("x"));
            Assert.AreEqual("WrappedSet(<z><p x=\"B\">A</p><p x=\"B\">A</p><q><h>1</h></q><q><h>2</h></q><q><h>C</h></q></z>)", set.ToString());
        }
    }

    public class ExampleTransformation : Transformation
    {
        protected override void Transform()
        {
            Text("p", "A");
            Attr("p", "x", "B");
            ReplaceAll("q",  new [] {"1", "2", "3"}, s => Text("h", s)).Last("h").Text("C");
        }
    }
}