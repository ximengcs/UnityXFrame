
namespace UnityXFrame.Core.UIs
{
    public class UIGroup : IUIGroup
    {
        public float Alpha { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public int Layer { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        float IUIGroup.Alpha { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        int IUIGroup.Layer { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public void Close()
        {
            throw new System.NotImplementedException();
        }

        public void Open()
        {
            throw new System.NotImplementedException();
        }

        void IUIGroup.Close()
        {
            throw new System.NotImplementedException();
        }

        void IUIGroup.CloseUI(IUI ui)
        {
            throw new System.NotImplementedException();
        }

        void IUIGroup.OnClose()
        {
            throw new System.NotImplementedException();
        }

        void IUIGroup.OnOpen()
        {
            throw new System.NotImplementedException();
        }

        void IUIGroup.Open()
        {
            throw new System.NotImplementedException();
        }

        void IUIGroup.OpenUI(IUI ui)
        {
            throw new System.NotImplementedException();
        }
    }
}
