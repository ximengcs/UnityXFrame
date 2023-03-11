
using UnityEditor;
using UnityEngine;

namespace UnityXFrame.Editor
{
    public class TestGUISkinWindow : EditorWindow
    {
        private Vector2 m_Pos;

        private void OnGUI()
        {
            m_Pos = GUILayout.BeginScrollView(m_Pos);
            GUIStyle[] skins = GUI.skin.customStyles;
            for (int i = 0; i < skins.Length; i++)
                GUILayout.Label($"{i}", skins[i]);
            GUILayout.EndScrollView();
        }
    }
}
