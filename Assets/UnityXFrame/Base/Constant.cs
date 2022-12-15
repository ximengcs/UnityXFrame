
using System.IO;
using UnityEngine;

namespace UnityXFrame.Core
{
    public static class Constant
    {
        public static string ArchivePath => Path.Combine(Application.persistentDataPath, "archive");

        public static int SIZE_X = 1080;
        public static int SIZE_Y = 1920;
        public static int UNIT_PIXEL = 100;
        public static int SCENEUI_SORT_LAYER = 0;
        public static string SCENEUI_RES_PATH = "Data/SceneUI";
    }
}
