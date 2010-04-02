using System;
using System.Xml.Linq;

namespace Neffigy.Mvc
{
    public class FileSystemXElementLoader : XElementLoader
    {
        public XElement Load(string path, Func<string, string> mapPath)
        {
            return XElement.Load(mapPath(path), LoadOptions.PreserveWhitespace);
        }
    }
}