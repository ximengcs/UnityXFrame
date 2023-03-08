using UnityEditor;
using UnityEngine;

namespace UnityXFrame.Editor
{
    public partial class InitEditor
    {
        private class LocalizeEditor : DataEditorBase
        {
            private TextAsset m_File;

            public override void OnUpdate()
            {
                EditorGUILayout.BeginHorizontal();
                Utility.Lable("LocalizeFile");
                m_File = (TextAsset)EditorGUILayout.ObjectField(m_Data.LocalizeFile, typeof(TextAsset), false);
                EditorGUILayout.EndHorizontal();

                if (m_File != m_Data.LocalizeFile)
                {
                    m_Data.LocalizeFile = m_File;
                    EditorUtility.SetDirty(m_Data);
                }
            }
        }
    }
}
