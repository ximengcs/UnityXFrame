using UnityEngine;
using UnityXFrame.Core;
using XFrame.Modules.XType;
using XFrame.Modules.Resource;
using XFrame.Modules.Procedure;
using XFrame.Modules.Diagnotics;
using UnityXFrame.Core.HotUpdate;

namespace Game.Core.Procedure
{
    public class EnterHotfixProcedure : ProcedureBase
    {
        protected override void OnEnter()
        {
            base.OnEnter();
            Log.Debug("EnterHotfixProcedure");
#if !HOTFIX_EDITOR
            ResModule.Inst.LoadAsync<TextAsset>(Constant.HOTFIX_PATH)
                .OnComplete((asset) => TypeModule.Inst.LoadAssembly(asset.bytes))
                .Start();
#else
            HotSetUpModule.Inst.StartHotProc();
#endif
        }
    }
}
