#if CONSOLE
using System.Text;
using UnityEngine;

namespace UnityXFrame.Core.Diagnotics
{
    [DebugWindow(true)]
    public class LogScaner : IDebugWindow
    {
        private StringBuilder m_Common;
        private StringBuilder m_Warning;
        private StringBuilder m_Error;

        private const int SCROLL_HEIGHT = 300;
        private Vector2 m_CommonScrollPos;
        private Vector2 m_WarningScrollPos;
        private Vector2 m_ErrorScrollPos;

        public void OnAwake()
        {
            m_Common = new StringBuilder();
            m_Warning = new StringBuilder();
            m_Error = new StringBuilder();
            Application.logMessageReceived += InternalLogCallback;
        }

        public void OnDraw()
        {
            InternalDrawKind("Common", m_Common, ref m_CommonScrollPos);
            InternalDrawKind("Warning", m_Warning, ref m_WarningScrollPos);
            InternalDrawKind("Error", m_Error, ref m_ErrorScrollPos);
        }

        public void Dispose()
        {
            Application.logMessageReceived -= InternalLogCallback;
            m_Common = null;
            m_Warning = null;
            m_Error = null;
        }

        private void InternalDrawKind(string kind, StringBuilder content, ref Vector2 scrollPos)
        {
            GUILayout.BeginHorizontal();
            DebugGUI.Label(kind);
            if (DebugGUI.Button("Copy"))
                GUIUtility.systemCopyBuffer = content.ToString();
            if (DebugGUI.Button("Clear"))
                content.Clear();
            GUILayout.EndHorizontal();
            scrollPos = GUILayout.BeginScrollView(scrollPos, false, true, GUILayout.Height(SCROLL_HEIGHT));
            DebugGUI.TextArea(content.ToString());
            GUILayout.EndScrollView();
            DebugGUI.Line();
        }

        private void InternalLogCallback(string condition, string stackTrace, LogType type)
        {
            switch (type)
            {
                case LogType.Log:
                    m_Common.Append(condition);
                    m_Common.Append("\n\n");
                    break;
                case LogType.Warning:
                    m_Warning.Append("<color=#CC9A06>");
                    m_Warning.Append(condition);
                    m_Warning.Append("\n");
                    m_Warning.Append(stackTrace);
                    m_Warning.Append("</color>\n\n");
                    break;
                case LogType.Error:
                case LogType.Assert:
                case LogType.Exception:
                    Debuger.Tip(this, "LogScanner has new error");
                    m_Error.Append("<color=#CC423B>");
                    m_Error.Append(condition);
                    m_Error.Append("\n");
                    m_Error.Append(stackTrace);
                    m_Error.Append("</color>\n\n");
                    break;
            }
        }
    }
}
#endif