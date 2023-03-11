using UnityEngine.UI;
using UnityXFrame.Core.UIs;
using XFrame.Modules.Diagnotics;

namespace Game.Test
{
    public class TestUI2 : MonoUI
    {
        public Button Btn;

        protected override void OnOpen(object data)
        {
            Log.Debug("OnOpen");
            Btn.onClick.AddListener(InnerClick);
        }

        protected override void OnClose()
        {
            Log.Debug("OnClose");
            Btn.onClick.RemoveListener(InnerClick);
        }

        private void InnerClick()
        {
            Log.Debug("OnClick");
        }
    }
}
