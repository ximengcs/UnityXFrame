using System;
using UnityEditor;
using UnityEngine;
using UnityXFrame.Core;
using XFrame.Modules.Resource;
using XFrame.Modules.XType;

namespace UnityXFrame.Editor
{
    public partial class InitEditor
    {
        private class ResModeEditor : IDataEditor
        {
            private InitData m_Data;
            private TypeModule.System m_ResHelperTypes;
            private Type[] m_Types;
            private string[] m_ResHelperTypeNames;
            private int m_ResHelperTypeIndex;

            public void OnInit(InitData data)
            {
                m_Data = data;
                m_ResHelperTypes = TypeModule.Inst.GetOrNew<IResourceHelper>();

                m_Types = m_ResHelperTypes.ToArray();
                m_ResHelperTypeNames = new string[m_Types.Length];
                for (int i = 0; i < m_Types.Length; i++)
                {
                    string name = m_Types[i].Name;
                    m_ResHelperTypeNames[i] = name;
                    if (name == m_Data.ResMode)
                        m_ResHelperTypeIndex = i;
                }

                if (string.IsNullOrEmpty(m_Data.ResMode) && m_ResHelperTypeNames.Length > 0)
                    InnerSelect(0);
            }

            public void OnUpdate()
            {
                EditorGUILayout.BeginHorizontal();
                Utility.Lable("Res Mode");
                int index = m_ResHelperTypeIndex;
                m_ResHelperTypeIndex = EditorGUILayout.Popup(m_ResHelperTypeIndex, m_ResHelperTypeNames);
                EditorGUILayout.EndHorizontal();

                if (index != m_ResHelperTypeIndex)
                    InnerSelect(m_ResHelperTypeIndex);
            }

            public void OnDestroy()
            {

            }

            private void InnerSelect(int index)
            {
                m_ResHelperTypeIndex = index;
                m_Data.ResMode = m_Types[m_ResHelperTypeIndex].FullName;
                EditorUtility.SetDirty(m_Data);
            }
        }
    }
}
