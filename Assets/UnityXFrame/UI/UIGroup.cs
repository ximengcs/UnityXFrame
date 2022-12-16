
using UnityEngine;

namespace UnityXFrame.Core.UIs
{
    internal class UIGroup : IUIGroup
    {
        private int m_Layer;
        private GameObject m_Root;

        public string Name { get; }

        public float Alpha { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public int Layer
        {
            get { return m_Layer; }
            set
            {
                value = Mathf.Min(value, UIModule.Inst.GroupCount);
                value = Mathf.Max(value, 0);
                m_Layer = value;
                UIModule.Inst.SetGroupLayer(this, value);
            }
        }

        public bool IsOpen => true;

        public UIGroup(GameObject root, string name, int layer)
        {
            m_Root = root;
            Name = name;
        }

        public void Close()
        {

        }

        public void Open()
        {

        }

        void IUIGroup.Close()
        {

        }

        void IUIGroup.OnCloseUI(IUI ui)
        {

        }

        void IUIGroup.Open()
        {

        }

        void IUIGroup.OnOpenUI(IUI ui)
        {

        }

        void IUIGroup.OnInit()
        {

        }

        void IUIGroup.OnUpdate()
        {
            
        }

        void IUIGroup.OnDestroy()
        {

        }
    }
}
