using System;
using System.Text;
using UnityEngine;
using XFrame.Modules.XType;

namespace UnityXFrame.Core.Diagnotics
{
    public class TypeScanner : IDebugWindow
    {
        private Vector2 m_Pos;
        private StringBuilder m_Str;

        public void OnAwake()
        {
            if (m_Str == null)
            {
                m_Str = new StringBuilder();
                InnerRefresh();
            }
        }

        public void OnDraw()
        {
            if (DebugGUI.Button("Refresh"))
                InnerRefresh();
            m_Pos = DebugGUI.BeginScrollView(m_Pos);
            GUILayout.Box(new GUIContent(m_Str.ToString()));
            GUILayout.EndScrollView();
        }

        public void Dispose()
        {

        }

        private void InnerRefresh()
        {
            m_Str.Clear();
            Type[] types = TypeModule.Inst.GetAllType();
            foreach (Type type in types)
            {
                m_Str.AppendLine(type.Name);
            }
        }
    }
}
