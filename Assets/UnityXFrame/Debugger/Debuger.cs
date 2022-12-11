using UnityEngine;

namespace UnityXFrame.Core
{
    public partial class Debuger : MonoBehaviour
    {
        private static Module m_Module;

        public static void SetModule(Module module)
        {
            m_Module = module;
        }

        #region Common Method
        public static void Tip(IDebugWindow from, string content, string color = null)
        {
            m_Module?.Tip(from, content, color);
        }
        #endregion

        private void OnGUI()
        {
            m_Module.OnDraw();
        }
    }
}