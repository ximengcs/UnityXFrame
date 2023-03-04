using System;
using UnityEngine;
using XFrame.Modules.Local;
using System.Collections.Generic;
using UnityEngine.Audio;

namespace UnityXFrame.Core
{
    public class InitData : ScriptableObject
    {
        public string Entrance;
        public Language Language;
        public string Logger;
        public string ResMode;
        public string ArchivePath;
        public string JsonSerializer;
        public TextAsset LocalizeFile;
        public GUISkin DebuggerSkin;
        public List<DebugColor> LogMark;
        public AudioMixer AudioMixer;
    }

    [Serializable]
    public class DebugColor
    {
        public string Key;
        public Color Color;
        public bool Value;
    }
}
