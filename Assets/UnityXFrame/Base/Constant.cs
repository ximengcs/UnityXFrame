
using System.IO;
using UnityEngine;

namespace UnityXFrame.Core
{
    public static class Constant
    {
        public static string ArchivePath => Path.Combine(Application.persistentDataPath, "archive");
    }
}
