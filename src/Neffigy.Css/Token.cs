namespace Neffigy.Css
{
    public abstract class Token
    {
        public string Value { get; set; }

        public override string ToString()
        {
            return string.Format("{0}:{1}", GetType().Name.Replace("Token", ""), Value);
        }
    }

    public class IdToken : Token
    {
    }

    public class AttributeValueToken : Token
    {
    }

    public class AttributeComparisonToken : Token
    {
    }

    public class ChildToken : Token
    {
    }

    public class ElementToken : Token
    {
    }

    public class ClassToken : Token
    {
    }

    public class WhiteSpaceToken : Token
    {
    }

    public class AttributeStartToken : Token
    {
    }

    public class AttributeEndToken : Token
    {
    }

    public class AttributeKeyToken : Token
    {
    }
}