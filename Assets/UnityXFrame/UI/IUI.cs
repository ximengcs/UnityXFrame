
namespace UnityXFrame.Core.UIs
{
    public interface IUI
    {
        float Alpha { get; set; }
        int Layer { get; set; }
        internal void OnOpen();
        internal void OnClose();
        void Open();
        void Close();
    }
}
