using System;
using System.Collections.Generic;
using System.Linq;
using Neffigy.Css;
using NUnit.Framework;

namespace Neffigy.UnitTests.Css
{
    [TestFixture]
    public class CssParserTests
    {
        static IEnumerable<Traversal> Parse(params Token[] tokens)
        {
            return new CssParser().Parse(tokens).ToArray();
        }

        static void AssertEquivalent(IEnumerable<Traversal> actual, params Traversal[] expected)
        {
            Assert.That(Stringify(actual.ToArray()), Is.EquivalentTo(Stringify(expected)));
        }

        private static IEnumerable<string> Stringify(IEnumerable<Traversal> traversals)
        {
            return traversals.Select(t => t.ToXPath("x"));
        }

        static void AssertThrowsInvalidAttribute(params Token[] tokens)
        {
            Assert.Throws<ArgumentException>(() => Parse(tokens), "Invalid attribute expression");
        }
        
        [Test]
        public void Element()
        {
            AssertEquivalent(Parse(
                                 new ElementToken { Value = "p" }),
                             new DescendantElements(),
                             new ElementNameEquals("p")
                );
        }

        [Test]
        public void Id()
        {
            AssertEquivalent(Parse(
                                 new IdToken { Value = "#bar" }),
                             new DescendantElements(),
                             new AttributeValueEquals("id", "bar")
                );
        }

        [Test]
        public void Class()
        {
            AssertEquivalent(Parse(
                                 new ClassToken { Value = ".bar" }),
                             new DescendantElements(),
                             new AttributeValueEquals("class", "bar")
                );
        }

        [Test]
        public void ElementWithId()
        {
            AssertEquivalent(Parse(
                                 new ElementToken { Value = "a" },
                                 new IdToken { Value = "#b" }),
                             new DescendantElements(),
                             new ElementNameEquals("a"),
                             new AttributeValueEquals("id", "b")
                );
        }

        [Test]
        public void ElementWithClass()
        {
            AssertEquivalent(Parse(
                                 new ElementToken { Value = "a" },
                                 new ClassToken { Value = ".b"}),
                             new DescendantElements(),
                             new ElementNameEquals("a"),
                             new AttributeValueEquals("class", "b")                
                );
        }

        [Test]
        public void ElementWithChildWithClass()
        {
            AssertEquivalent(Parse(
                                 new ElementToken { Value = "a" },
                                 new ChildToken { Value = ">" },
                                 new ClassToken { Value = ".b" }),
                             new DescendantElements(),
                             new ElementNameEquals("a"),
                             new ChildElements(),
                             new AttributeValueEquals("class", "b")
                );
        }

        [Test]
        public void ElementWithChildWithClassLeadingWhiteSpace()
        {
            AssertEquivalent(Parse(
                                 new ElementToken { Value = "a" },
                                 new WhiteSpaceToken { Value = " " },
                                 new ChildToken { Value = ">" },
                                 new ClassToken { Value = ".b" }),
                             new DescendantElements(),
                             new ElementNameEquals("a"),
                             new ChildElements(),
                             new AttributeValueEquals("class", "b")
                );
        }

        [Test]
        public void ElementWithChildWithClassTrailingWhiteSpace()
        {
            AssertEquivalent(Parse(
                                 new ElementToken { Value = "a" },
                                 new ChildToken { Value = ">" },
                                 new WhiteSpaceToken { Value = " " },
                                 new ClassToken { Value = ".b" }),
                             new DescendantElements(),
                             new ElementNameEquals("a"),
                             new ChildElements(),
                             new AttributeValueEquals("class", "b")
                );
        }

        [Test]
        public void ElementWithChildWithClassLeadingAndTrailingWhiteSpace()
        {
            AssertEquivalent(Parse(
                                 new ElementToken { Value = "a" },
                                 new WhiteSpaceToken { Value = " " },
                                 new ChildToken { Value = ">" },
                                 new WhiteSpaceToken { Value = " " },
                                 new ClassToken { Value = ".b" }),
                             new DescendantElements(),
                             new ElementNameEquals("a"),
                             new ChildElements(),
                             new AttributeValueEquals("class", "b")
                );
        }

        [Test]
        public void ElementWithDescendantWithClass()
        {
            AssertEquivalent(Parse(
                                 new ElementToken { Value = "a" },
                                 new WhiteSpaceToken { Value = " " },
                                 new ClassToken { Value = ".b" }),
                             new DescendantElements(),
                             new ElementNameEquals("a"),
                             new DescendantElements(),
                             new AttributeValueEquals("class", "b")
                );
        }

        [Test]
        public void HasAttribute()
        {
            AssertEquivalent(Parse(
                                 new AttributeStartToken { Value = "[" },
                                 new AttributeKeyToken { Value = "z" },
                                 new AttributeEndToken { Value = "]" }),
                             new HasAttribute("z")
                );
        }

        [Test]
        public void AttributeValueEquals()
        {
            AssertEquivalent(Parse(
                                 new AttributeStartToken { Value = "[" },
                                 new AttributeKeyToken { Value = "x" },
                                 new AttributeComparisonToken { Value = "=" },
                                 new AttributeValueToken { Value = "y" },
                                 new AttributeEndToken { Value = "]" }),
                             new AttributeValueEquals("x", "y")
                );
        }

        [Test]
        public void InvalidAttributeExpression()
        {
            AssertThrowsInvalidAttribute(new AttributeStartToken { Value = "[" });
            AssertThrowsInvalidAttribute(new AttributeStartToken { Value = "[" }, new AttributeEndToken { Value = "]" });
            AssertThrowsInvalidAttribute(
                new AttributeStartToken { Value = "[" },
                new AttributeKeyToken { Value = "a" },
                new AttributeComparisonToken { Value = "=" }
                );
        }
    }
}