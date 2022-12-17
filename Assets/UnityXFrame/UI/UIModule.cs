using System;
using System.IO;
using UnityEngine;
using XFrame.Core;
using XFrame.Collections;
using XFrame.Modules.Resource;
using UnityXFrame.Core.Resource;
using XFrame.Modules.Diagnotics;
using System.Collections.Generic;
using System.Diagnostics;

namespace UnityXFrame.Core.UIs
{
    public partial class UIModule : SingletonModule<UIModule>
    {
        private Canvas m_Canvas;
        private Transform m_Root;
        private Dictionary<Type, IUI> m_UIMap;
        private Dictionary<Type, IUIFactory> m_Factorys;
        private XLinkList<IUIGroup> m_GroupList;

        public int GroupCount => m_GroupList.Count;

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
                m_Factorys = new Dictionary<Type, IUIFactory>();
                AddFactory<UI, UI.Factory>();
                AddFactory<MonoUI, MonoUI.Factory>();
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

        #region Open UI
        public IUI Open(Type type, object data = default, bool useNavtive = false)
        {
            return Open(Constant.MAIN_GROUPUI, type, data, useNavtive);
        }

        public T Open<T>(object data = default, bool useNavtive = false) where T : IUI
        {
            return (T)Open(typeof(T), data, useNavtive);
        }

        public T Open<T>(string groupName, object data = default, bool useNavtive = false) where T : IUI
        {
            return (T)Open(groupName, typeof(T), data, useNavtive);
        }

        public IUI Open(string groupName, Type type, object data = default, bool useNavtive = false)
        {
            IUIGroup group = InnerGetOrNewGroup(groupName, m_GroupList.Count);
            return InnerOpenUI(group, type, data, useNavtive);
        }

        public IUI Open(IUIGroup group, Type type, object data = default, bool useNavtive = false)
        {
            return InnerOpenUI(group, type, data, useNavtive);
        }

        public T Open<T>(IUIGroup group, object data = default, bool useNavtive = false)
        {
            return (T)InnerOpenUI(group, typeof(T), data, useNavtive);
        }
        #endregion

        #region Close UI
        public void Close<T>() where T : IUI
        {
            InnerCloseUI(typeof(T));
        }

        public void Close(Type uiType)
        {
            InnerCloseUI(uiType);
        }
        #endregion

        public void AddFactory<UIType, T>() where UIType : IUI where T : IUIFactory
        {
            Type uiType = typeof(UIType);
            if (m_Factorys.ContainsKey(uiType))
                return;
            m_Factorys[uiType] = (T)Activator.CreateInstance(typeof(T));
        }

        public IUIGroup GetOrNew(string groupName, int layer = -1)
        {
            if (layer == -1)
                layer = m_GroupList.Count;
            return InnerGetOrNewGroup(groupName, layer);
        }
        #endregion

        #region Inner Implement
        private IUIFactory InnerGetUIFactory(Type uiType)
        {
            if (m_Factorys.TryGetValue(uiType.BaseType, out IUIFactory factory))
                return factory;
            return default;
        }

        private void InnerCloseUI(Type uiTyp)
        {
            if (m_UIMap.TryGetValue(uiTyp, out IUI ui))
                ui.Close();
        }

        private IUI InnerOpenUI(IUIGroup group, Type uiType, object data, bool useNavtive)
        {
            if (!m_UIMap.TryGetValue(uiType, out IUI ui))
            {
                GameObject prefab;
                string uiPath = Path.Combine(Constant.UI_RES_PATH, uiType.Name);

                if (useNavtive)
                    prefab = NativeResModule.Inst.Load<GameObject>(uiPath);
                else
                    prefab = ResModule.Inst.Load<GameObject>(uiPath);


                if (prefab == null)
                {
                    Log.Error(nameof(UIModule), $"UI res {uiPath} dont exist.");
                    return default;
                }
                Stopwatch sw = Stopwatch.StartNew();
                sw.Start();
                GameObject inst = GameObject.Instantiate(prefab);
                sw.Stop();
                Log.Debug(sw.ElapsedMilliseconds);
                inst.name = uiType.Name;
                IUIFactory factory = InnerGetUIFactory(uiType);

                ui = factory.Create(inst, uiType);
                ui.OnInit(inst);
                m_UIMap[uiType] = ui;
            }

            IUIGroup oldGroup = ui.Group;
            if (oldGroup != group)
            {
                oldGroup?.RemoveUI(ui);
                ui.OnGroupChange(group);
                group.AddUI(ui);
            }
            ui.Open(data);
            group.Open();
            return ui;
        }

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

        internal void SetUIGroupLayer(IUIGroup group, int layer)
        {
            SetLayer(m_Root, group, layer);
        }
        #endregion
    }
}
