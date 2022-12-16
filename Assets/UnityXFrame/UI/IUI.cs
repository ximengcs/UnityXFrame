
namespace UnityXFrame.Core.UIs
{
    public interface IUI
    {
        bool IsOpen { get; }
        int Layer { get; set; }
        protected internal void OnOpen();
        protected internal void OnClose();
        protected internal void OnInit();
        protected internal void OnGroupChange(IUIGroup newGroup);
        protected internal void OnReset(object data);
        protected internal void OnUpdate();
        protected internal void OnDestroy();
        void Open();
        void Close();
    }
}
