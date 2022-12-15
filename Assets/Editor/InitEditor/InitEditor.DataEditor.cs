
using UnityXFrame.Core;

namespace UnityXFrame.Editor
{
    public partial class InitEditor
    {
        private interface IDataEditor
        {
            void OnInit(InitData data);
            void OnUpdate();
            void OnDestroy();
        }
    }
}
