using System.Diagnostics;
using UnityXFrame.Core.Diagnotics;
using UnityXFrame.Core.UIs;
using XFrame.Modules.Diagnotics;

namespace Game.Test
{
    [DebugWindow]
    public class TestFrame : IDebugWindow
    {
        public void Dispose()
        {

        }

        public void OnAwake()
        {

        }

        public void OnDraw()
        {
            if (DebugGUI.Button("Open UI 1"))
            {
                UIModule.Inst.Open<TestUI1>(default, true);
            }
            if (DebugGUI.Button("Close UI 1"))
                UIModule.Inst.Close<TestUI1>();
            if (DebugGUI.Button("Open UI 2"))
                UIModule.Inst.Open<TestUI2>(default, true);
            if (DebugGUI.Button("Close UI 2"))
                UIModule.Inst.Close<TestUI2>();
        }
    }
}
