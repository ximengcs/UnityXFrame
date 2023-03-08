using UnityEditor;
using UnityEngine;

namespace UnityXFrame.Editor
{
    public partial class InitEditor
    {
        private class Utility
        {
            public static void Lable(string lable)
            {
                EditorGUILayout.LabelField(lable, GUI.skin.customStyles[40], GUILayout.Width(100));
            }

            public static bool Toggle(string lable, bool vale)
            {
                return EditorGUILayout.Toggle(lable, vale, GUI.skin.customStyles[148]);
            }

            public static bool Toggle(bool vale)
            {
                return EditorGUILayout.Toggle(vale, GUI.skin.customStyles[148]);
            }
        }
    }
}
