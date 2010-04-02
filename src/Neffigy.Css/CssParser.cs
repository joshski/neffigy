using System;
using System.Collections.Generic;

namespace Neffigy.Css
{
    public class CssParser
    {
        public IEnumerable<Traversal> Parse(IEnumerable<Token> tokens)
        {
            // yuck, look away ;-)
            var enumerator = tokens.GetEnumerator();
            Traversal descend = new DescendantElements();
            var ignoreWhite = false;
            while (enumerator.MoveNext()) {
                var token = enumerator.Current;
                if (token is ElementToken)
                {
                    if (descend != null)
                        yield return descend;
                    descend = null; 
                    yield return new ElementNameEquals(token.Value);
                }
                else if (token is ClassToken)
                {
                    if (descend != null)
                        yield return descend;
                    descend = null;
                    yield return new AttributeValueEquals("class", token.Value.Substring(1));
                }
                else if (token is IdToken)
                {
                    if (descend != null)
                        yield return descend;
                    descend = null;
                    yield return new AttributeValueEquals("id", token.Value.Substring(1));
                }
                else if (token is ChildToken)
                {
                    descend = null;
                    yield return new ChildElements();
                    ignoreWhite = true;
                }
                else if (token is AttributeStartToken)
                {
                    descend = null;
                    yield return ParseAttribute(enumerator);
                }
                else if (token is WhiteSpaceToken && !ignoreWhite)
                {
                    descend = new DescendantElements();
                }
                if (!(token is ChildToken))
                    ignoreWhite = false;
            }
        }

        static Traversal ParseAttribute(IEnumerator<Token> enumerator)
        {
            if (enumerator.MoveNext())
            {
                var key = enumerator.Current;
                if (enumerator.MoveNext())
                {
                    var next = enumerator.Current;
                    if (next is AttributeEndToken)
                    {
                        return new HasAttribute(key.Value);
                    }
                    if (next is AttributeComparisonToken)
                    {
                        if (enumerator.MoveNext())
                        {
                            var next2 = enumerator.Current;
                            if (next2 is AttributeValueToken)
                            {
                                return new AttributeValueEquals(key.Value, next2.Value);
                            }
                        }
                    }

                }
            }
            throw new ArgumentException("Invalid attribute expression");
        }
    }
}
