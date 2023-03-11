using HybridCLR;
using UnityEditor;

namespace UnityXFrame.Editor
{
    public partial class InitEditor
    {
        private class HotfixEditor : DataEditorBase
        {
            public override void OnUpdate()
            {
                base.OnUpdate();

                EditorGUILayout.BeginHorizontal();
                Utility.Lable("AOTMetaPath");
                m_Data.AOTMetaDllPath = EditorGUILayout.TextField(m_Data.AOTMetaDllPath);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                Utility.Lable("HotfixDllPath");
                m_Data.HotfixDllPath = EditorGUILayout.TextField(m_Data.HotfixDllPath);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                Utility.Lable("Load Mode");
                m_Data.AOTMetaMode = (HomologousImageMode)EditorGUILayout.EnumPopup(m_Data.AOTMetaMode);
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}
