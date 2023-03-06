using System;
using UnityEditor;
using UnityXFrame.Core;
using XFrame.Modules.Procedure;
using XFrame.Modules.XType;

namespace UnityXFrame.Editor
{
    public partial class InitEditor
    {
        private class ProcedureEditor : DataEditorBase
        {
            private TypeModule.System m_ProcTypes;
            private Type[] Types;
            private string[] m_ProcTypeNames;
            private int m_TypeIndex;

            protected override void OnInit()
            {
                m_ProcTypes = TypeModule.Inst.GetOrNew<ProcedureBase>();

                Types = m_ProcTypes.ToArray();
                m_ProcTypeNames = new string[Types.Length];
                for (int i = 0; i < Types.Length; i++)
                {
                    string name = Types[i].Name;
                    m_ProcTypeNames[i] = name;
                    if (name == m_Data.Entrance)
                        m_TypeIndex = i;
                }

                if (string.IsNullOrEmpty(m_Data.Entrance) && m_ProcTypeNames.Length > 0)
                    InnerSelect(0);
            }

            public override void OnUpdate()
            {
                EditorGUILayout.BeginHorizontal();
                Utility.Lable("Entrance");
                int index = m_TypeIndex;
                m_TypeIndex = EditorGUILayout.Popup(m_TypeIndex, m_ProcTypeNames);
                if (index != m_TypeIndex)
                    InnerSelect(m_TypeIndex);
                EditorGUILayout.EndHorizontal();
            }

            private void InnerSelect(int index)
            {
                m_TypeIndex = index;
                m_Data.Entrance = Types[m_TypeIndex].FullName;
                EditorUtility.SetDirty(m_Data);
            }
        }
    }

}
