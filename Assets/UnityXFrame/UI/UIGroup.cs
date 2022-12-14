using UnityEngine;
using XFrame.Collections;

namespace UnityXFrame.Core.UIs
{
    public class UIGroup : IUIGroup
    {
        private int m_Layer;
        private GameObject m_Inst;
        private Transform m_Root;
        private CanvasGroup m_CanvasGroup;
        private XLinkList<IUI> m_UIs;

        public string Name { get; }
        public bool IsOpen { get; private set; }
        public float Alpha
        {
            get => m_CanvasGroup.alpha;
            set => m_CanvasGroup.alpha = value;
        }

        public int Layer
        {
            get { return m_Layer; }
            set
            {
                m_Layer = value;
                UIModule.Inst.SetUIGroupLayer(this, value);
            }
        }

        public UIGroup(GameObject root, string name, int layer)
        {
            Name = name;
            Layer = layer;
            m_Inst = root;
            m_Root = root.transform;
            m_UIs = new XLinkList<IUI>();
            m_CanvasGroup = root.GetComponent<CanvasGroup>();

            RectTransform rectTf = m_Root as RectTransform;
            rectTf.anchorMin = Vector3.zero;
            rectTf.anchorMax = Vector3.one;
            rectTf.offsetMin = Vector2.zero;
            rectTf.offsetMax = Vector2.zero;
            rectTf.anchoredPosition3D = Vector3.zero;
        }

        public void Open()
        {
            if (IsOpen)
                return;
            IsOpen = true;
            m_Inst.gameObject.SetActive(true);
        }

        public void Close()
        {
            if (!IsOpen)
                return;
            IsOpen = false;
            m_Inst.gameObject.SetActive(false);
        }

        void IUIGroup.CloseUI(IUI ui)
        {
            ui.OnClose();
        }

        void IUIGroup.OpenUI(IUI ui, object data)
        {
            ui.OnOpen(data);
        }

        void IUIGroup.OnInit()
        {
            IsOpen = true;
            Close();
        }

        void IUIGroup.OnUpdate()
        {
            if (IsOpen)
            {
                XLinkNode<IUI> node = m_UIs.First;
                while (node != null)
                {
                    node.Value.OnUpdate();
                    node = node.Next;
                }
            }
        }

        void IUIGroup.OnDestroy()
        {
            XLinkNode<IUI> node = m_UIs.First;
            while (node != null)
            {
                node.Value.OnDestroy();
                node = node.Next;
            }
        }

        void IUIGroup.AddUI(IUI ui)
        {
            ui.Root.SetParent(m_Root, false);
            m_UIs.AddFirst(ui);
            ui.SetLayer(m_UIs.Count - 1, false);
        }

        void IUIGroup.RemoveUI(IUI ui)
        {
            ui.Root.SetParent(null, false);

            bool reach = false;
            XLinkNode<IUI> node = m_UIs.First;
            while (node != null)
            {
                IUI other = node.Value;
                if (reach)
                    ui.SetLayer(other.Layer - 1, false);

                if (other == ui)
                {
                    node.Delete();
                    reach = true;
                }
                node = node.Next;
            }
        }

        void IUIGroup.SetUILayer(IUI ui, int layer)
        {
            layer = Mathf.Min(layer, m_UIs.Count - 1);
            layer = Mathf.Max(layer, 0);
            UIModule.SetLayer(m_Root, ui, layer);
        }
    }
}
