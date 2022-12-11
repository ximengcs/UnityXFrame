﻿#if UNITY_EDITOR
using UnityEditor.AI;

namespace UnityXFrame.Core
{
    [DebugWindow]
    public class EditorTool : IDebugWindow
    {
        private int m_ShowNav;

        public void OnAwake()
        {
            m_ShowNav = NavMeshVisualizationSettings.showNavigation;
        }

        public void OnDraw()
        {
            if (DebugGUI.Button("Show Nav"))
            {
                NavMeshVisualizationSettings.showNavigation++;
                Debuger.Tip(this, "Show Nav Mesh");
            }
        }

        public void Dispose()
        {
            NavMeshVisualizationSettings.showNavigation = m_ShowNav;
        }
    }
}
#endif