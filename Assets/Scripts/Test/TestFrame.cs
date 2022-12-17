using UnityXFrame.Core.UIs;
using UnityXFrame.Core.Diagnotics;

namespace Game.Test
{
    [DebugWindow]
    public class TestFrame : IDebugWindow
    {
        private int m_UI = 1;
        private int m_Layer;

        public void Dispose()
        {

        }

        public void OnAwake()
        {

        }

        public void OnDraw()
        {
            m_UI = DebugGUI.IntField(m_UI);
            m_Layer = DebugGUI.IntField(m_Layer);
            if (DebugGUI.Button("Open UI"))
                UIModule.Inst.Open($"TestUI{m_UI}", default, true);
            if (DebugGUI.Button("Close UI"))
                UIModule.Inst.Close($"TestUI{m_UI}");
            if (DebugGUI.Button("Set Layer"))
                UIModule.Inst.Get($"TestUI{m_UI}").Layer = m_Layer;
        }
    }
}
