using System;
using UnityEngine;
using XFrame.Core;
using XFrame.Collections;
using System.Collections.Generic;
using XFrame.Modules.Diagnotics;

namespace UnityXFrame.Core.UIs
{
    public class UIModule : SingletonModule<UIModule>
    {
        private Canvas m_Canvas;
        private Transform m_Root;
        private Dictionary<Type, IUI> m_UIMap;
        private XLinkList<IUIGroup> m_GroupList;

        internal int GroupCount => m_GroupList.Count;

        #region Life Fun
        protected override void OnInit(object data)
        {
            base.OnInit(data);

            InnerCheckCanvas(data);
            if (m_Canvas != null)
            {
                m_Root = m_Canvas.transform;
                m_UIMap = new Dictionary<Type, IUI>();
                m_GroupList = new XLinkList<IUIGroup>();
            }
        }

        protected override void OnUpdate(float escapeTime)
        {
            base.OnUpdate(escapeTime);
            XLinkNode<IUIGroup> node = m_GroupList.First;
            while (node != null)
            {
                if (node.Value.IsOpen)
                    node.Value.OnUpdate();
                node = node.Next;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            XLinkNode<IUIGroup> node = m_GroupList.First;
            while (node != null)
            {
                node.Value.OnDestroy();
                node = node.Next;
            }
            m_GroupList.Dispose();
            m_GroupList = null;
        }
        #endregion

        #region Interface
        public IUIGroup MainGroup
        {
            get { return InnerGetOrNewGroup(Constant.MAIN_GROUPUI, GroupCount); }
        }

        public IUI Open(Type type, object data = default)
        {
            return Open(Constant.MAIN_GROUPUI, type, data);
        }

        public T Open<T>(object data = default) where T : IUI
        {
            return (T)Open(typeof(T), data);
        }

        public IUI Open(string groupName, Type type, object data = default)
        {
            IUIGroup group = InnerGetOrNewGroup(groupName, m_GroupList.Count);
            if (!m_UIMap.TryGetValue(type, out IUI ui))
            {
                ui = (IUI)Activator.CreateInstance(type);
                ui.OnInit();
                ui.OnGroupChange(group);
                m_UIMap[type] = ui;
            }

            ui.OnReset(data);
            ui.Open();
            group.Open();
            return ui;
        }

        public IUIGroup GetOrNew(string groupName, int layer = -1)
        {
            if (layer == -1)
                layer = m_GroupList.Count;
            return InnerGetOrNewGroup(groupName, layer);
        }
        #endregion

        #region Inner Implement
        private void InnerCheckCanvas(object canvas)
        {
            if (canvas != null)
                m_Canvas = (Canvas)canvas;
            if (m_Canvas == null)
                m_Canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        }

        private IUIGroup InnerGetOrNewGroup(string groupName, int layer)
        {
            XLinkNode<IUIGroup> node = m_GroupList.First;
            while (node != null)
            {
                if (node.Value.Name == groupName)
                    return node.Value;
                node = node.Next;
            }

            GameObject groupRoot = new GameObject(groupName, typeof(RectTransform), typeof(CanvasGroup));
            groupRoot.transform.SetParent(m_Root);
            IUIGroup group = new UIGroup(groupRoot, groupName, layer);
            group.OnInit();
            m_GroupList.AddLast(group);
            return group;
        }

        internal void SetGroupLayer(IUIGroup group, int layer)
        {
            bool find = false;
            Transform[] list = new Transform[m_Root.childCount];


            int curIndex = 0;
            for (int i = 0; i < list.Length; i++, curIndex++)
            {
                Transform child = m_Root.GetChild(i);
                if (!find && child.name == group.Name)
                {
                    find = true;
                    list[layer] = child;
                    if (layer != curIndex)
                        curIndex--;
                }
                else
                {
                    if (layer == curIndex)
                        curIndex++;
                    list[curIndex] = child;
                }
            }

            m_Root.DetachChildren();
            foreach (Transform child in list)
            {
                child.SetParent(m_Root);
                Debug.LogWarning(child.name + " " + child.parent.name);
            }
        }
        #endregion
    }
}
