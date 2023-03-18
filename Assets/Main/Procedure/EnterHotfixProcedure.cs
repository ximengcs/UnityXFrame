using UnityEngine;
using UnityXFrame.Core;
using XFrame.Modules.XType;
using XFrame.Modules.Resource;
using XFrame.Modules.Procedure;
using XFrame.Modules.Diagnotics;

namespace Game.Core.Procedure
{
    public class EnterHotfixProcedure : ProcedureBase
    {
        protected override void OnEnter()
        {
            base.OnEnter();
            Log.Debug("EnterHotfixProcedure");
            ResModule.Inst.LoadAsync<TextAsset>(Constant.HOTFIX_PATH)
                .OnComplete((asset) => TypeModule.Inst.LoadAssembly(asset.bytes))
                .Start();
        }
    }
}
