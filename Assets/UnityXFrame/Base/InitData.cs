using System;
using UnityEngine;
using XFrame.Modules.Local;
using System.Collections.Generic;

namespace UnityXFrame.Core
{
    public class InitData : ScriptableObject
    {
        public Language Language;
        public string Logger;
        public string ResMode;
        public string ArchivePath;
        public string JsonSerializer;
        public TextAsset LocalizeFile;
        public GUISkin DebuggerSkin;
        public List<InitDataDic> LogMark;
    }

    [Serializable]
    public class InitDataDic
    {
        public string Key;
        public Color Color;
        public bool Value;
    }
}
