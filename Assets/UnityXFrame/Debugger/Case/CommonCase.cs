using UnityXFrame.Core.UIs;

namespace UnityXFrame.Core.Diagnotics
{
    public class CommonCase : IDebugWindow
    {
        private int m_Layer;
        private string m_Name;

        public void Dispose()
        {

        }

        public void OnAwake()
        {

        }

        public void OnDraw()
        {
            m_Layer = DebugGUI.IntField(m_Layer);
            m_Name = DebugGUI.TextField(m_Name);
            if (DebugGUI.Button("Init"))
            {
                UIModule.Inst.GetOrNewGroup(m_Name);
            }

            if (DebugGUI.Button("Init2"))
            {
                UIModule.Inst.GetOrNewGroup(m_Name, m_Layer);
            }

            if (DebugGUI.Button("Set Layer"))
            {
                UIModule.Inst.GetOrNewGroup(m_Name).Layer = m_Layer;
            }
        }
    }
}