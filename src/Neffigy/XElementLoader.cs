using System;
using System.Xml.Linq;

namespace Neffigy
{
    public interface XElementLoader
    {
        XElement Load(string path, Func<string, string> mapPath);
    }
}