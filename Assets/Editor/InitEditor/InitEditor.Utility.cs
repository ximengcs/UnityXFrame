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
        }
    }
}
