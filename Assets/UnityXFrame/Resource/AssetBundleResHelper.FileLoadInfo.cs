
using System.IO;

namespace UnityXFrame.Core
{
    public partial class AssetBundleResHelper
    {
        private class FileLoadInfo
        {
            public string Name;
            public string NameWithoutExt;
            public string Extension;
            public BundleInfo Bundle;

            public FileLoadInfo(string filePath, BundleInfo bundle)
            {
                Bundle = bundle;
                string dir = Path.GetDirectoryName(filePath);
                NameWithoutExt = Path.Combine(dir, Path.GetFileNameWithoutExtension(filePath)).ToLower();
                Extension = Path.GetExtension(filePath).ToLower();
                Name = filePath.ToLower();
            }
        }
    }
}
