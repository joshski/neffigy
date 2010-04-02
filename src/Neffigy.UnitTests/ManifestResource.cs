using System.IO;

namespace Neffigy.UnitTests
{
    public static class ManifestResource
    {
        public static string Read(string name)
        {
            using (var stream = typeof(ManifestResource).Assembly.GetManifestResourceStream(typeof(ManifestResource), name))
            {
                using (var reader = new StreamReader(stream))
                        return reader.ReadToEnd();
            }
        }
    }
}
