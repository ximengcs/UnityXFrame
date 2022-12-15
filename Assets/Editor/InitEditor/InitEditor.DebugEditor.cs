using System;
using UnityEditor;
using UnityXFrame.Core;
using XFrame.Modules.XType;
using XFrame.Modules.Diagnotics;
using UnityEngine;
using System.Collections.Generic;

namespace UnityXFrame.Editor
{
    public partial class InitEditor
    {
        private class DebugEditor : IDataEditor
        {
            private InitData m_Data;
            private TypeModule.System m_LogHelperTypes;
            private Type[] m_Types;
            private string[] m_LogHelperTypeNames;
            private int m_LogHelperTypeIndex;
            private Vector2 m_LogScrollPos;
            private GUISkin m_DebuggerSkin;

            public void OnInit(InitData data)
            {
                m_Data = data;
                m_LogHelperTypes = TypeModule.Inst.GetOrNew<XFrame.Modules.Diagnotics.ILogger>();

                m_Types = m_LogHelperTypes.ToArray();
                m_LogHelperTypeNames = new string[m_Types.Length];
                for (int i = 0; i < m_Types.Length; i++)
                {
                    string name = m_Types[i].Name;
                    m_LogHelperTypeNames[i] = name;
                    if (name == m_Data.ResMode)
                        m_LogHelperTypeIndex = i;
                }

                if (string.IsNullOrEmpty(m_Data.Logger) && m_LogHelperTypeNames.Length > 0)
                    InnerSelect(0);

                m_DebuggerSkin = m_Data.DebuggerSkin;
            }

            public void OnUpdate()
            {
                EditorGUILayout.BeginHorizontal();
                Utility.Lable("Logger");
                int index = m_LogHelperTypeIndex;
                m_LogHelperTypeIndex = EditorGUILayout.Popup(m_LogHelperTypeIndex, m_LogHelperTypeNames);
                if (index != m_LogHelperTypeIndex)
                    InnerSelect(m_LogHelperTypeIndex);
                EditorGUILayout.EndHorizontal();


                #region All Color Select
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Colors", GUI.skin.customStyles[312]);
                bool all = true;
                bool dirty = false;
                foreach (DebugColor data in m_Data.LogMark)
                {
                    if (!data.Value)
                    {
                        all = false;
                        break;
                    }
                }
                bool olds = all;
                all = EditorGUILayout.Toggle(all);
                if (olds != all)
                    dirty = true;
                EditorGUILayout.EndHorizontal();
                #endregion

                m_LogScrollPos = EditorGUILayout.BeginScrollView(m_LogScrollPos);
                List<int> willDel = new List<int>();
                for (int i = 0; i < m_Data.LogMark.Count; i++)
                {
                    DebugColor logMark = m_Data.LogMark[i];
                    if (dirty) logMark.Value = all;
                    EditorGUILayout.BeginHorizontal();
                    logMark.Key = EditorGUILayout.TextField(logMark.Key);
                    logMark.Color = EditorGUILayout.ColorField(logMark.Color);
                    logMark.Value = EditorGUILayout.Toggle(logMark.Value);
                    if (GUILayout.Button("-"))
                        willDel.Add(i);
                    EditorGUILayout.EndHorizontal();
                }
                if (GUILayout.Button("+"))
                {
                    DebugColor data = new DebugColor();
                    m_Data.LogMark.Add(data);
                }

                foreach (int i in willDel)
                    m_Data.LogMark.RemoveAt(i);
                EditorGUILayout.EndScrollView();

                EditorGUILayout.BeginHorizontal();
                Utility.Lable("DebuggerSkin");
                m_DebuggerSkin = (GUISkin)EditorGUILayout.ObjectField(m_DebuggerSkin, typeof(GUISkin), false);

                if (m_DebuggerSkin != null && m_DebuggerSkin != m_Data.DebuggerSkin)
                {
                    m_Data.DebuggerSkin = m_DebuggerSkin;
                    EditorUtility.SetDirty(m_Data);
                }
                EditorGUILayout.EndHorizontal();
            }

            public void OnDestroy()
            {

            }

            private void InnerSelect(int index)
            {
                m_LogHelperTypeIndex = index;
                m_Data.Logger = m_Types[m_LogHelperTypeIndex].FullName;
                EditorUtility.SetDirty(m_Data);
            }
        }
    }
}
