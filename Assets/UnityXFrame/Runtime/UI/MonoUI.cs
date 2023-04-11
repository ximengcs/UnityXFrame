using System;
using UnityEngine;
using XFrame.Modules.Containers;
using XFrame.Modules.Diagnotics;

namespace UnityXFrame.Core.UIs
{
    public abstract partial class MonoUI : MonoBehaviour, IUI
    {
        private IContainer m_Container;

        protected bool m_IsOpen;
        protected int Layer;
        protected IUIGroup m_Group;
        protected GameObject m_Root;
        protected Transform m_Transform;

        int IUI.Layer
        {
            get { return Layer; }
            set
            {
                Layer = value;
                m_Group?.SetUILayer(this, Layer);
            }
        }

        bool IUI.IsOpen => m_IsOpen;

        IUIGroup IUI.Group => m_Group;

        public Transform Root => m_Transform;

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
            GameObject.Destroy(m_Root);
        }

        void IUI.OnGroupChange(IUIGroup newGroup)
        {
            m_Group = newGroup as UIGroup;
        }

        void IUI.OnInit(GameObject inst)
        {
            m_Container = ContainerModule.Inst.New(this);
            m_Transform = inst.transform;
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
            m_Root.gameObject.SetActive(true);
            OnOpen();
        }

        void IUI.OnClose()
        {
            m_Root.gameObject.SetActive(false);
            OnClose();
        }

        void IUI.SetLayer(int layer, bool refresh)
        {
            Layer = layer;
            if (refresh)
                m_Group?.SetUILayer(this, Layer);
        }

        protected virtual void OnInit() { }
        protected virtual void OnUpdate() { }
        protected virtual void OnOpen() { }
        protected virtual void OnClose() { }


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
            m_Container.Dispose();
        }
        #endregion
    }
}
