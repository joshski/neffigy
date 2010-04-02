using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Neffigy.Css
{
    public class CssTokeniser
    {
        private readonly StringReader reader;
        private Func<IEnumerable<Token>> currentState;

        public CssTokeniser(StringReader reader)
        {
            this.reader = reader;
            currentState = Normal;
        }

        public IEnumerable<Token> Tokenise()
        {
            while (currentState != null)
            {
                var tokens = currentState();
                foreach (var token in tokens)
                {
                    yield return token;
                }
            }
        }

        IEnumerable<Token> Normal()
        {
            var r = reader.Peek();
            if (r > -1) {
                return Normal((char)r);
            }
            SetState(null);
            return Enumerable.Empty<Token>();
        }

        IEnumerable<Token> Normal(char ch)
        {
            if (ch == '.')
                SetState(StartClassName);
            else if (ch == '[')
                SetState(StartAttribute);
            else if (ch == '#')
                SetState(Id);
            else if (ch == '>')
                SetState(Child);
            else if (IsIdentifier(ch))
                SetState(ElementName);
            else if (Char.IsWhiteSpace(ch))
                SetState(WhiteSpace);
            else
                throw new ArgumentException(string.Format("Unexpected character '{0}' in CSS selector", ch));
            return currentState();
        }

        void SetState(Func<IEnumerable<Token>> state)
        {
            currentState = state;
        }

        IEnumerable<Token> Child()
        {
            yield return new ChildToken { Value = Read().ToString() };
            SetState(Normal);
        }

        IEnumerable<Token> Id()
        {
            yield return new IdToken { Value = Read() + ReadWhile(IsIdentifier) };
            SetState(Normal);
        }

        IEnumerable<Token> StartAttribute()
        {
            yield return new AttributeStartToken { Value = Read().ToString() };
            if (EndOfString)
                SetState(Normal);
            else
                SetState(AttributeKey);
        }

        IEnumerable<Token> AttributeKey()
        {
            yield return new AttributeKeyToken { Value = ReadWhile(Char.IsLetterOrDigit) };
            if (Peek() == ']')
                SetState(AttributeEnd);
            else if (EndOfString)
                SetState(Normal);
            else
                SetState(AttributeComparison);
        }



        IEnumerable<Token> AttributeComparison()
        {
            yield return new AttributeComparisonToken { Value = Read().ToString() };
            if (EndOfString)
                SetState(Normal);
            else
                SetState(AttributeValue);
        }

        IEnumerable<Token> AttributeValue()
        {
            if (Peek() != ']')
            {
                yield return new AttributeValueToken { Value = ReadWhile(c => c != ']') };
                if (EndOfString)
                {
                    SetState(Normal);
                    yield break;
                }
            }
            SetState(AttributeEnd);
        }

        IEnumerable<Token> AttributeEnd()
        {
            yield return new AttributeEndToken { Value = Read().ToString() };
            SetState(Normal);
        }

        IEnumerable<Token> ElementName()
        {
            yield return new ElementToken { Value = ReadWhile(IsIdentifier) };
            SetState(Normal);
        }

        IEnumerable<Token> StartClassName()
        {
            yield return new ClassToken { Value = Read() + ReadWhile(IsIdentifier) };
            SetState(Normal);
        }

        IEnumerable<Token> WhiteSpace()
        {
            yield return new WhiteSpaceToken { Value = Read() + ReadWhile(Char.IsWhiteSpace) };
            SetState(Normal);
        }

        char Read()
        {
            return (char) reader.Read();
        }

        char Peek()
        {
            return (char)reader.Peek();
        }

        string ReadWhile(Func<char, bool> condition)
        {
            var str = "";
            while (EndOfString == false && condition(Peek()))
            {
                str += Read(); 
            }
            return str;
        }

        bool EndOfString
        {
            get { return reader.Peek() == -1; }
        }

        static bool IsIdentifier(char ch)
        {
            return Char.IsLetterOrDigit(ch) || ch == '-' || ch == '_';
        }
    }
}