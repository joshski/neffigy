namespace Neffigy.Css
{
    public abstract class Traversal
    {
        public abstract string ToXPath(string ns);
    }

    public class AttributeValueEquals : Traversal
    {
        private readonly string key;
        private readonly string value;

        public AttributeValueEquals(string key, string value)
        {
            this.key = key;
            this.value = value;
        }

        public override string ToXPath(string ns)
        {
            return string.Format("[@{0}='{1}']", key, value);
        }
    }

    public class HasAttribute : Traversal
    {
        private readonly string name;

        public HasAttribute(string name)
        {
            this.name = name;
        }

        public override string ToXPath(string ns)
        {
            return "[@" + name + "]";
        }
    }

    public class ChildElements : Traversal
    {
        public override string ToXPath(string ns)
        {
            return "/";
        }
    }

    public class DescendantElements : Traversal
    {
        public override string ToXPath(string ns)
        {
            return "//";
        }
    }

    public class ElementNameEquals : Traversal
    {
        private readonly string name;

        public ElementNameEquals(string name)
        {
            this.name = name;
        }

        public override string ToXPath(string ns)
        {
            return ns == null ? name : ns + ":" + name;
        }
    }
}