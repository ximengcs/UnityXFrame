using System;
using UnityEditor;
using UnityXFrame.Core;
using XFrame.Modules.Serialize;
using XFrame.Modules.XType;

namespace UnityXFrame.Editor
{
    public partial class InitEditor
    {
        private class SerializeEditor : IDataEditor
        {
            private InitData m_Data;
            private TypeModule.System m_HelperTypes;
            private Type[] Types;
            private string[] m_HelperTypeNames;
            private int m_HelperTypeIndex;

            public void OnInit(InitData data)
            {
                m_Data = data;
                m_HelperTypes = TypeModule.Inst.GetOrNew<IJsonSerializeHelper>();

                Types = m_HelperTypes.ToArray();
                m_HelperTypeNames = new string[Types.Length];
                for (int i = 0; i < Types.Length; i++)
                {
                    string name = Types[i].Name;
                    m_HelperTypeNames[i] = name;
                    if (name == m_Data.ResMode)
                        m_HelperTypeIndex = i;
                }

                if (string.IsNullOrEmpty(m_Data.JsonSerializer) && m_HelperTypeNames.Length > 0)
                    InnerSelect(0);
            }

            public void OnUpdate()
            {
                EditorGUILayout.BeginHorizontal();
                Utility.Lable("JsonSerializer");
                int index = m_HelperTypeIndex;
                m_HelperTypeIndex = EditorGUILayout.Popup(m_HelperTypeIndex, m_HelperTypeNames);
                if (index != m_HelperTypeIndex)
                    InnerSelect(m_HelperTypeIndex);
                EditorGUILayout.EndHorizontal();
            }

            public void OnDestroy()
            {

            }

            private void InnerSelect(int index)
            {
                m_HelperTypeIndex = index;
                m_Data.JsonSerializer = Types[m_HelperTypeIndex].FullName;
                EditorUtility.SetDirty(m_Data);
            }
        }
    }
}
