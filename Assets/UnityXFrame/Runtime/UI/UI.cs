using System;
using System.Collections.Generic;
using UnityEngine;
using XFrame.Collections;
using XFrame.Modules.Containers;
using XFrame.Modules.Diagnotics;

namespace UnityXFrame.Core.UIs
{
    /// <summary>
    /// UI基类
    /// </summary>
    public abstract partial class UI : IUI
    {
        private int m_Id;
        private IContainer m_Container;

        protected bool m_IsOpen;
        protected int m_Layer;
        protected IUIGroup m_Group;
        protected GameObject m_Root;
        protected RectTransform m_Transform;

        #region UI Interface
        int IUI.Layer
        {
            get { return m_Layer; }
            set { ((IUI)this).SetLayer(value, true); }
        }

        bool IUI.Active
        {
            get => m_Root.activeSelf;
            set => m_Root.SetActive(value);
        }

        bool IUI.IsOpen => m_IsOpen;

        IUIGroup IUI.Group => m_Group;

        public int Id => m_Id;

        public RectTransform Root => m_Transform;

        public string Name => m_Root.name;

        public void Open()
        {
            if (m_IsOpen)
                return;
            m_IsOpen = true;
            IUIGroup group = m_Group;
            if (group != null)
            {
                group.OpenUI(this);
            }
            else
            {
                Log.Error(nameof(UIModule), "UI Group is null.");
            }
        }

        public void Close()
        {
            if (!m_IsOpen)
                return;
            m_IsOpen = false;
            IUIGroup group = m_Group;
            if (group != null)
            {
                group.CloseUI(this);
            }
            else
            {
                Log.Error(nameof(UIModule), "UI Group is null.");
            }
        }

        void IUI.OnDestroy()
        {
            ContainerModule.Inst.Remove(m_Container);
            m_Container = null;
            GameObject.Destroy(m_Root);
        }

        public UI()
        {
            m_Container = ContainerModule.Inst.New(this);
        }

        void IUI.OnGroupChange(IUIGroup newGroup)
        {
            m_Group = newGroup as UIGroup;
        }

        void IUI.OnInit(int id, GameObject inst)
        {
            m_Id = id;
            m_Transform = inst.GetComponent<RectTransform>();
            m_Root = inst;
            OnInit();
        }

        void IUI.OnUpdate()
        {
            if (m_IsOpen)
                OnUpdate();
        }

        void IUI.OnOpen()
        {
            m_Container.Dispatch((com) =>
            {
                UICom uiCom = com as UICom;
                if (uiCom != null)
                    uiCom.OnOpen();
            });
            OnOpen();
        }

        void IUI.OnClose()
        {
            m_Container.Dispatch((com) =>
            {
                UICom uiCom = com as UICom;
                if (uiCom != null)
                    uiCom.OnClose();
            });
            OnClose();
        }

        void IUI.SetLayer(int layer, bool refresh)
        {
            m_Layer = layer;
            if (refresh)
                m_Group?.SetUILayer(this, m_Layer);
        }
        #endregion

        #region Sub Class Implement Life Fun
        protected virtual void OnInit() { }
        protected virtual void OnUpdate() { }
        protected virtual void OnOpen() { }
        protected virtual void OnClose() { }
        #endregion

        #region Container Interface
        public T Get<T>(int id = 0) where T : ICom
        {
            return m_Container.Get<T>(id);
        }

        public ICom Get(Type type, int id = 0)
        {
            return m_Container.Get(type, id);
        }

        public T Add<T>(Action<ICom> comInitComplete = null) where T : ICom
        {
            return m_Container.Add<T>(comInitComplete);
        }

        public T Add<T>(int id, Action<ICom> comInitComplete = null) where T : ICom
        {
            return m_Container.Add<T>(id, comInitComplete);
        }

        public ICom Add(Type type, Action<ICom> comInitComplete = null)
        {
            return m_Container.Add(type, comInitComplete);
        }

        public ICom Add(Type type, int id = 0, Action<ICom> comInitComplete = null)
        {
            return m_Container.Add(type, id, comInitComplete);
        }

        public T GetOrAdd<T>(Action<ICom> comInitComplete = null) where T : ICom
        {
            return m_Container.GetOrAdd<T>(comInitComplete);
        }

        public T GetOrAdd<T>(int id = 0, Action<ICom> comInitComplete = null) where T : ICom
        {
            return m_Container.GetOrAdd<T>(id, comInitComplete);
        }

        public ICom GetOrAdd(Type type, Action<ICom> comInitComplete = null)
        {
            return m_Container.GetOrAdd(type, comInitComplete);
        }

        public ICom GetOrAdd(Type type, int id = 0, Action<ICom> comInitComplete = null)
        {
            return m_Container.GetOrAdd(type, id, comInitComplete);
        }

        public void Remove<T>(int id = 0) where T : ICom
        {
            m_Container.Remove<T>(id);
        }

        public void Remove(Type type, int id = 0)
        {
            m_Container.Remove(type, id);
        }

        public void Clear()
        {
            m_Container.Clear();
        }

        public void Dispatch(Action<ICom> handle)
        {
            m_Container.Dispatch(handle);
        }

        public void SetData<T>(T value)
        {
            m_Container.SetData(value);
        }

        public T GetData<T>()
        {
            return m_Container.GetData<T>();
        }

        public void SetData<T>(string name, T value)
        {
            m_Container.SetData<T>(name, value);
        }

        public T GetData<T>(string name)
        {
            return m_Container.GetData<T>(name);
        }

        public void Dispose()
        {
            ContainerModule.Inst.Remove(m_Container);
            m_Container = null;
        }

        public IEnumerator<ICom> GetEnumerator()
        {
            return m_Container.GetEnumerator();
        }

        public void SetIt(XItType type)
        {
            m_Container.SetIt(type);
        }
        #endregion
    }
}
