using UnityXFrame.Core.UIs;
using UnityXFrame.Core.Diagnotics;
using Newtonsoft.Json.Linq;

namespace Game.Test
{
    public class TestFrame : IDebugWindow
    {
        private int m_Group;
        private int m_UI = 1;
        private int m_Layer;
        private int m_GroupLayer;

        public void Dispose()
        {

        }

        public void OnAwake()
        {

        }

        public void OnDraw()
        {
            m_UI = DebugGUI.IntField(m_UI);
            m_Group = DebugGUI.IntField(m_Group);
            m_Layer = DebugGUI.IntField(m_Layer);
            m_GroupLayer = DebugGUI.IntField(m_GroupLayer);
            if (DebugGUI.Button("Open UI"))
                UIModule.Inst.Open($"TestUI{m_UI}", default, true);
            if (DebugGUI.Button("Close UI"))
                UIModule.Inst.Close($"TestUI{m_UI}");
            if(DebugGUI.Button("Open UI To Group"))
                UIModule.Inst.Open($"TestUI{m_UI}", $"Group{m_UI}", default, true);
            if (DebugGUI.Button("Set Layer"))
                UIModule.Inst.Get($"TestUI{m_UI}").Layer = m_Layer;
            if (DebugGUI.Button("Set Group Layer"))
                UIModule.Inst.MainGroup.Layer = m_GroupLayer;
        }
    }
}
