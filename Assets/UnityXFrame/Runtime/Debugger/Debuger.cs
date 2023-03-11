#if CONSOLE
using System;
using UnityEngine;
using XFrame.Core;
using System.Reflection;
using XFrame.Modules.Times;
using XFrame.Modules.XType;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace UnityXFrame.Core.Diagnotics
{
    [XModule]
    public partial class Debuger : SingletonModule<Debuger>
    {
        #region Internal Field
        private GUISkin Skin;
        private GUIStyle m_TitleStyle;
        private GUIStyle m_CloseButtonStyle;
        private GUIStyle m_ContentStyle;
        private GUIStyle m_TipTitleStyle;
        private GUIStyle m_TipContentStyle;
        private GUIStyle m_DebugArea;
        private GUIStyle m_MenuArea;
        private GUIStyle m_ContentArea;
        private GUIStyle m_HelpWindowStyle;

        private bool m_IsOpen;
        private bool m_HelpOpen;
        private Rect m_RootRect;
        private bool m_OnGUIInit;
        private Rect m_HelpRect;
        private Vector2 m_ContentPos;
        private Vector2 m_DebugMenuPos;
        private List<WindowInfo> m_Windows;
        private WindowInfo m_Current;

        private TweenModule m_TweenModule;
        private HashSet<int> m_TipNewMsg;
        private bool m_AlwaysTip;
        private string m_Tip;
        private CDTimer m_Timer;

        private EventSystem m_EventSytem;

        private const string TITLE = "Console";
        private const int TIP_CD_KEY = 0;
        private const int TIP_CD = 3;
        #endregion

        public void SetTip(IDebugWindow from, string content, string color = null)
        {
            if (!string.IsNullOrEmpty(color))
                content = $"<color=#{color.Trim('#')}>{content}</color>";

            m_Tip = content;
            m_Timer.Reset(TIP_CD_KEY);
            m_Timer.Check(TIP_CD_KEY, true);

            int code = from.GetHashCode();
            if (!m_TipNewMsg.Contains(code))
                m_TipNewMsg.Add(code);
        }

        #region Life Fun
        protected override void OnInit(object data)
        {
            base.OnInit(data);
            m_OnGUIInit = false;
            Skin = Init.Inst.Data.DebuggerSkin;
            m_CloseButtonStyle = Skin.customStyles[0];
            m_TitleStyle = Skin.customStyles[1];
            m_ContentStyle = Skin.customStyles[2];

            DebugGUI.Style = new DebugStyle();
            DebugGUI.Style.Skin = Skin;
            DebugGUI.Style.Button = Skin.customStyles[3];
            DebugGUI.Style.Text = Skin.customStyles[4];
            DebugGUI.Style.Lable = Skin.customStyles[5];
            DebugGUI.Style.TextArea = Skin.customStyles[6];
            m_TipTitleStyle = Skin.customStyles[7];
            m_TipContentStyle = Skin.customStyles[8];
            m_DebugArea = Skin.customStyles[9];
            DebugGUI.Style.Toolbar = Skin.customStyles[10];
            m_MenuArea = Skin.customStyles[11];
            m_ContentArea = Skin.customStyles[12];
            m_HelpWindowStyle = Skin.customStyles[13];

            m_TweenModule = new TweenModule();
            m_Timer = new CDTimer();
            m_Timer.Record(TIP_CD_KEY, TIP_CD);
            m_TipNewMsg = new HashSet<int>();

            m_Windows = new List<WindowInfo>();
            InternalLoadInst();
            m_Windows.Sort((info1, info2) => info1.Order - info2.Order);

            if (m_Windows.Count > 0)
                InternalSelectMenu(m_Windows[0]);
        }

        private void InnerGUIInit()
        {
            GUI.skin.verticalScrollbarThumb = Skin.verticalScrollbarThumb;
            GUI.skin.horizontalScrollbarThumb = Skin.horizontalScrollbarThumb;
            GUI.skin.box = Skin.box;
            Skin.window.fixedWidth = Screen.width;
            Skin.window.fixedHeight = Mathf.Min(Skin.window.fixedHeight, Screen.height);
            m_HelpWindowStyle.fixedWidth = Skin.window.fixedWidth;
            m_HelpRect.y = Skin.window.fixedHeight;
            m_HelpWindowStyle.fixedHeight = 0;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            foreach (WindowInfo info in m_Windows)
                info.Window.Dispose();
            m_Windows = null;
        }

        public void OnGUI()
        {
            if (!m_OnGUIInit)
            {
                m_OnGUIInit = true;
                InnerGUIInit();
            }

            InternalCheckInGUI();
            if (m_IsOpen)
            {
                if (m_HelpWindowStyle.fixedHeight > 0)
                {
                    m_HelpRect = GUILayout.Window(1, m_HelpRect, InternalDrawHelpWindow, string.Empty, m_HelpWindowStyle);
                }
                m_RootRect = GUILayout.Window(0, m_RootRect, InternalDrawRootWindow, string.Empty, Skin.window);
                m_TweenModule.OnUpdate();
            }
            else
            {
                string title = TITLE;
                if (m_TipNewMsg.Count > 0)
                    title = $"<color=#FF0000>{title}</color>";
                if (GUILayout.Button(title, Skin.button))
                    m_IsOpen = true;
            }
        }
        #endregion

        private Vector2 scrollPos;
        private void InternalDrawHelpWindow(int windowId)
        {
            if (m_HelpWindowStyle.fixedHeight < m_RootRect.height)
                return;
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Help", m_TitleStyle);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            scrollPos = DebugGUI.BeginScrollView(scrollPos);
            GUILayout.Box(m_Current.HelpInfo);
            GUILayout.EndScrollView();
        }

        #region Internal Implement
        private void InternalCheckInGUI()
        {
            if (m_EventSytem == null)
            {
                GameObject inst = GameObject.Find("EventSystem");
                if (inst != null)
                    m_EventSytem = inst.GetComponent<EventSystem>();
            }
            else
            {
                Vector3 touchPos = Input.mousePosition;
                touchPos.y = Screen.height - touchPos.y;
                bool enable = m_IsOpen ? !m_RootRect.Contains(touchPos) : true;
                m_EventSytem.enabled = enable;
            }
        }

        private void InternalLoadInst()
        {
            TypeSystem typeSys = TypeModule.Inst.GetOrNew<IDebugWindow>();
            foreach (Type t in typeSys)
            {
                DebugWindowAttribute atr = t.GetCustomAttribute<DebugWindowAttribute>();

                WindowInfo info = new WindowInfo();
                if (atr != null)
                {
                    info.Name = atr.Name;
                    info.AlwaysRun = atr.AlwaysRun;
                    info.Order = atr.Order;
                }
                else
                {
                    info.Name = default;
                    info.AlwaysRun = default;
                    info.Order = default;
                }
                if (string.IsNullOrEmpty(info.Name))
                    info.Name = t.Name.Replace("Case", string.Empty);

                DebugHelpAttribute helpAtr = t.GetCustomAttribute<DebugHelpAttribute>();
                if (helpAtr != null)
                    info.HelpInfo = helpAtr.Content;
                else
                    info.HelpInfo = "No help information";

                IDebugWindow window = Activator.CreateInstance(t) as IDebugWindow;
                info.Window = window;
                m_Windows.Add(info);

                if (info.AlwaysRun)
                    window.OnAwake();
            }
        }

        private void InnerClose()
        {
            m_IsOpen = false;
            m_HelpOpen = false;
            m_HelpWindowStyle.fixedHeight = 0;
        }

        private void InternalDrawRootWindow(int windowId)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(TITLE, m_TitleStyle);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("X", m_CloseButtonStyle))
                InnerClose();
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical(m_DebugArea);
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            GUILayout.BeginVertical(m_MenuArea);

            m_DebugMenuPos = DebugGUI.BeginScrollView(m_DebugMenuPos);
            foreach (WindowInfo windowInfo in m_Windows)
            {
                string title = windowInfo.Name;
                int code = windowInfo.Window.GetHashCode();
                if (m_TipNewMsg.Contains(code))
                    title = $"<color=#FF0000>{title}</color>";
                if (windowInfo.Window == m_Current.Window)
                    title = $"<color=#2A89FF>{title}</color>";

                if (GUILayout.Button(title, Skin.button))
                    InternalSelectMenu(windowInfo);

                code = m_Current.Window.GetHashCode();
                if (m_TipNewMsg.Contains(code))
                    m_TipNewMsg.Remove(code);
            }
            GUILayout.EndScrollView();
            GUILayout.EndVertical();

            GUILayout.BeginVertical(m_ContentArea);
            m_ContentPos = DebugGUI.BeginScrollView(m_ContentPos);
            m_Current.Window?.OnDraw();
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("?", m_TipTitleStyle))
            {
                m_HelpOpen = !m_HelpOpen;
                float target = m_HelpOpen ? m_RootRect.height : 0;
                m_TweenModule.Do("?", target, 0.1f,
                    () => m_HelpWindowStyle.fixedHeight,
                    (v) => m_HelpWindowStyle.fixedHeight = v);
            }
            if (GUILayout.Button("Tip", m_TipTitleStyle))
                m_AlwaysTip = !m_AlwaysTip;
            if (!m_AlwaysTip && m_Timer.Check(TIP_CD_KEY, true))
                m_Tip = string.Empty;
            GUILayout.Label(m_Tip, m_TipContentStyle);
            GUILayout.EndHorizontal();
        }

        private void InternalSelectMenu(WindowInfo info)
        {
            if (m_Current.Window != null && !m_Current.AlwaysRun)
                m_Current.Window?.Dispose();
            m_Current = info;
            if (m_Current.Window != null && !m_Current.AlwaysRun)
                m_Current.Window.OnAwake();
        }

        private struct WindowInfo
        {
            public string Name;
            public int Order;
            public bool AlwaysRun;
            public IDebugWindow Window;
            public string HelpInfo;
        }
        #endregion
    }
}
#endif