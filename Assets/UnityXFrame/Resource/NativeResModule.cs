using XFrame.Core;
using XFrame.Modules.Resource;

namespace UnityXFrame.Core.Resource
{
    /// <summary>
    /// 本地资源加载 (Resources)
    /// </summary>
    [CoreModule]
    public partial class NativeResModule : ResModule
    {
        public override int Id => 1;

        protected override void OnInit(object data)
        {
            InnerSetHelper(typeof(ResourcesHelper));
        }
    }
}
