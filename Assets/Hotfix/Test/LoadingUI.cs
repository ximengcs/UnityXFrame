using UnityXFrame.Core.UIs;

namespace Game.Test
{
    public partial class LoadingUI : UI
    {
        protected override void OnInit()
        {
            base.OnInit();
            UICommonCom com = Add<UICommonCom>();
            com.Add("Progress");
            Add<TestCom>();
        }

        protected override void OnOpen()
        {
            base.OnOpen();
        }
    }
}
