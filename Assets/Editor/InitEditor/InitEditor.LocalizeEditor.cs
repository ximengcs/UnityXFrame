using UnityEditor;
using UnityEngine;
using UnityXFrame.Core;

namespace UnityXFrame.Editor
{
    public partial class InitEditor
    {
        private class LocalizeEditor : IDataEditor
        {
            private InitData m_Data;
            private TextAsset m_File;

            public void OnInit(InitData data)
            {
                m_Data = data;
            }

            public void OnUpdate()
            {
                EditorGUILayout.BeginHorizontal();
                Utility.Lable("LocalizeFile");
                m_File = (TextAsset)EditorGUILayout.ObjectField(m_File, typeof(TextAsset), false);
                EditorGUILayout.EndHorizontal();

                if (m_File != m_Data.LocalizeFile)
                {
                    m_Data.LocalizeFile = m_File;
                    EditorUtility.SetDirty(m_Data);
                }
            }

            public void OnDestroy()
            {

            }
        }
    }
}
