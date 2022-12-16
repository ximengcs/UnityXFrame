
namespace UnityXFrame.Core.UIs
{
    public interface IUIGroup
    {
        string Name { get; }
        float Alpha { get; set; }
        int Layer { get; set; }
        internal void OpenUI(IUI ui);
        internal void CloseUI(IUI ui);
        internal void OnOpen();
        internal void OnClose();
        void Open();
        void Close();
    }
}
