using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Neffigy.UnitTests
{
    public class StubXElementLoader : Dictionary<string, string>, XElementLoader
    {
        public XElement Load(string path, Func<string, string> mapPath)
        {
            return XElement.Parse(mapPath(this[path]));
        }
    }
}