using System.Collections.Generic;
using UnityEngine;

namespace UnityXFrame.Core.Diagnotics
{
    public static class DebugGUI
    {
        public static DebugStyle Style;
        private static Dictionary<int, string> s_FloatTexts = new Dictionary<int, string>();

        public static void Line()
        {
            GUILayout.Label(string.Empty, GUILayout.Height(10));
            GUILayout.Button(string.Empty, GUILayout.Height(6));
        }

        public static bool Button(string title, params GUILayoutOption[] options)
        {
            return GUILayout.Button(title, Style.Button, options);
        }

        public static string TextField(string text, params GUILayoutOption[] options)
        {
            return GUILayout.TextField(text, Style.Text, options);
        }

        public static int IntField(int value, params GUILayoutOption[] options)
        {
            string valueText = value.ToString();
            valueText = TextField(valueText, options);
            int.TryParse(valueText, out value);
            return value;
        }

        public static float FloatField(float value, params GUILayoutOption[] options)
        {
            int code = GUIUtility.GetControlID(nameof(FloatField).GetHashCode(), FocusType.Keyboard, GUILayoutUtility.GetLastRect());
            string valueText;
            if (!s_FloatTexts.TryGetValue(code, out valueText))
                valueText = value.ToString();
            valueText = TextField(valueText, options);
            s_FloatTexts[code] = valueText;
            float.TryParse(valueText, out value);
            return value;
        }

        public static void Label(string content, params GUILayoutOption[] options)
        {
            GUILayout.Label(content, Style.Lable, options);
        }

        public static string TextArea(string content, params GUILayoutOption[] options)
        {
            return GUILayout.TextArea(content, Style.TextArea, options);
        }

        public static int Toolbar(int selectId, string[] texts, params GUILayoutOption[] options)
        {
            return GUILayout.Toolbar(selectId, texts, Style.Toolbar, options);
        }
    }
}