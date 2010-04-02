using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Neffigy.Css;
using NUnit.Framework;

namespace Neffigy.UnitTests.Css
{
    [TestFixture]
    public class CssTokeniserTests
    {
        static Token[] Tokenise(string selector)
        {
            return new CssTokeniser(new StringReader(selector)).Tokenise().ToArray();
        }

        static void AssertTokenises(string selector, params Token[] expectedTokens)
        {
            Assert.That(Stringify(Tokenise(selector)), Is.EquivalentTo(Stringify(expectedTokens)));           
        }

        private static IEnumerable<string> Stringify(IEnumerable<Token> tokens)
        {
            return tokens.Select(t => t.ToString()).ToArray();
        }

        [Test]
        public void Element()
        {
            AssertTokenises("p-z",new ElementToken { Value = "p-z" });
        }

        [Test]
        public void WhiteSpace()
        {
            AssertTokenises(" ", new WhiteSpaceToken { Value = " " });
            AssertTokenises("  ", new WhiteSpaceToken { Value = "  " });
            AssertTokenises("\t", new WhiteSpaceToken { Value = "\t" });
            AssertTokenises("\t ", new WhiteSpaceToken { Value = "\t " });
        }

        [Test]
        public void Class()
        {
            AssertTokenises(".foo", new ClassToken { Value = ".foo" });
            AssertTokenises("a.foo", new ElementToken { Value = "a" }, new ClassToken { Value = ".foo" });
        }

        [Test]
        public void Id()
        {
            AssertTokenises("#bar", new IdToken { Value = "#bar" });
        }

        [Test]
        public void Attribute()
        {
            AssertTokenises("[", new AttributeStartToken { Value = "[" });
            
            AssertTokenises("[boo",
                            new AttributeStartToken      { Value = "[" },
                            new AttributeKeyToken        { Value = "boo" });
            
            AssertTokenises("[foo=",
                            new AttributeStartToken      { Value = "[" },
                            new AttributeKeyToken        { Value = "foo" },
                            new AttributeComparisonToken { Value = "=" });

            AssertTokenises("[foo=value",
                            new AttributeStartToken { Value = "[" },
                            new AttributeKeyToken { Value = "foo" },
                            new AttributeComparisonToken { Value = "=" },
                            new AttributeValueToken { Value = "value"});

            AssertTokenises("[foo=]",
                            new AttributeStartToken      { Value = "[" },
                            new AttributeKeyToken        { Value = "foo" },
                            new AttributeComparisonToken { Value = "=" },
                            new AttributeEndToken        { Value = "]" });

            AssertTokenises("[foo=123]",
                            new AttributeStartToken      { Value = "[" },
                            new AttributeKeyToken        { Value = "foo" },
                            new AttributeComparisonToken { Value = "=" },
                            new AttributeValueToken      { Value = "123" },
                            new AttributeEndToken        { Value = "]" });
        }

        [Test]
        public void Child()
        {
            AssertTokenises("a>",
                            new ElementToken             { Value = "a" },
                            new ChildToken               { Value = ">" });

            AssertTokenises("a>b >c",
                            new ElementToken             { Value = "a" },
                            new ChildToken               { Value = ">" },
                            new ElementToken             { Value = "b" },
                            new ChildToken               { Value = ">" },
                            new WhiteSpaceToken          { Value = " " },
                            new ElementToken             { Value = "c" });
        }

        [Test]
        public void AllTokens()
        {
            AssertTokenises("ab p.blah[whatever] > #foo[another=bar]\t#last",
                            new ElementToken               { Value = "ab" },
                            new WhiteSpaceToken            { Value = " " },
                            new ElementToken               { Value = "p" },
                            new ClassToken                 { Value = ".blah" },
                            new AttributeStartToken        { Value = "[" },
                            new AttributeKeyToken          { Value = "whatever"},
                            new AttributeEndToken          { Value = "]"},
                            new WhiteSpaceToken            { Value = " " },
                            new ChildToken                 { Value = ">" },
                            new WhiteSpaceToken            { Value = " " },
                            new IdToken                    { Value = "#foo" },
                            new AttributeStartToken        { Value = "[" },
                            new AttributeKeyToken          { Value = "another"},
                            new AttributeComparisonToken   { Value = "="},
                            new AttributeValueToken        { Value = "bar"},
                            new AttributeEndToken          { Value = "]"},
                            new WhiteSpaceToken            { Value = "\t" },
                            new IdToken                    { Value = "#last" }
                );
        }

        [Test]
        public void Nonsense()
        {
            Assert.Throws<ArgumentException>(() => Tokenise("?"), "Unexpected character");
            Assert.Throws<ArgumentException>(() => Tokenise("!"), "Unexpected character");
        }
    }
}