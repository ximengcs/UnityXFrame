using HybridCLR;
using UnityEditor;
using UnityEngine;

namespace UnityXFrame.Editor
{
    public partial class InitEditor
    {
        private class HotfixEditor : DataEditorBase
        {
            private const string USE_EDITOR_SCRIPT = "HOTFIX_EDITOR";

            public override void OnUpdate()
            {
                base.OnUpdate();

                #region DebuggerSkin
                EditorGUILayout.BeginHorizontal();
                Utility.Lable("UseEditor");
                int old = InnerIsUseEditor() ? 0 : 1;
                int useEditor = GUILayout.Toolbar(old, new string[] { "On", "Off" });
                if (useEditor != old)
                    InnerSaveUseEditor(useEditor == 0 ? true : false);
                EditorGUILayout.EndHorizontal();
                #endregion

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

            private bool InnerIsUseEditor()
            {
                return Utility.ContainsSymbol(USE_EDITOR_SCRIPT);
            }

            private void InnerSaveUseEditor(bool debug)
            {
                if (debug)
                {
                    if (!Utility.ContainsSymbol(USE_EDITOR_SCRIPT))
                        Utility.AddSymbol(USE_EDITOR_SCRIPT);
                }
                else
                {
                    if (Utility.ContainsSymbol(USE_EDITOR_SCRIPT))
                        Utility.RemoveSymbol(USE_EDITOR_SCRIPT);
                }
            }
        }
    }
}
