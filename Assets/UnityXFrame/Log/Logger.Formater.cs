using UnityEngine;
using System.Collections.Generic;

namespace UnityXFrame.Core.Diagnotics
{
    public partial class Logger
    {
        private class Formater
        {
            private const int MAX_LENGTH = 1024;
            private Dictionary<string, Color> m_Colors;

            public Formater()
            {
                m_Colors = new Dictionary<string, Color>();
            }

            public void Register(string name, Color color)
            {
                m_Colors.Add(name, color);
            }

            public bool Format(string name, string content, out string result)
            {
                if (m_Colors.TryGetValue(name, out Color color))
                {
                    string colorHex = ColorUtility.ToHtmlStringRGB(color);
                    if (content.Length > MAX_LENGTH)
                        content = content.Substring(0, MAX_LENGTH);
                    content = $"<color=#00FFFF>[{name}]</color> <color=#{colorHex}>{content}</color>";
                    result = content;
                    return true;
                }
                else
                {
                    result = content;
                    return false;
                }
            }
        }
    }
}
