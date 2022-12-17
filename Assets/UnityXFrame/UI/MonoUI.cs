using UnityEngine;
using XFrame.Modules.Diagnotics;

namespace UnityXFrame.Core.UIs
{
    public abstract partial class MonoUI : MonoBehaviour, IUI
    {
        protected bool m_IsOpen;
        protected int Layer;
        protected UIGroup m_Group;
        protected GameObject m_Root;
        protected Transform m_Transform;

        int IUI.Layer { get; set; }

        bool IUI.IsOpen => m_IsOpen;

        IUIGroup IUI.Group => m_Group;

        public Transform Root => m_Transform;

        public string Name => m_Root.name;

        public void Open(object data)
        {
            if (m_IsOpen)
                return;
            m_IsOpen = true;
            IUIGroup group = m_Group;
            if (group != null)
            {
                group.OpenUI(this, data);
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
            m_Transform = inst.transform;
            m_Root = inst;
            OnInit();
        }

        void IUI.OnUpdate()
        {
            if (m_IsOpen)
                OnUpdate();
        }

        void IUI.OnOpen(object data)
        {
            m_Root.gameObject.SetActive(true);
            OnOpen(data);
        }

        void IUI.OnClose()
        {
            m_Root.gameObject.SetActive(false);
            OnClose();
        }

        protected virtual void OnInit() { }
        protected virtual void OnUpdate() { }
        protected virtual void OnOpen(object data) { }
        protected virtual void OnClose() { }
    }
}
