using UnityEditor;
using UnityXFrame.Core;
using XFrame.Modules.Local;

namespace UnityXFrame.Editor
{
    public partial class InitEditor
    {
        private class LangEditor : IDataEditor
        {
            private InitData m_Data;

            public void OnInit(InitData data)
            {
                m_Data = data;
            }

            public void OnUpdate()
            {
                EditorGUILayout.BeginHorizontal();
                Utility.Lable("Language");
                Language lang = m_Data.Language;
                m_Data.Language = (Language)EditorGUILayout.EnumPopup(m_Data.Language);
                if (m_Data.Language != lang)
                    EditorUtility.SetDirty(m_Data);
                EditorGUILayout.EndHorizontal();
            }

            public void OnDestroy()
            {

            }
        }
    }
}
