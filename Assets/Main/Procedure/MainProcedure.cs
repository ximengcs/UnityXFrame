using UnityXFrame.Core;
using XFrame.Modules.Tasks;
using XFrame.Modules.Procedure;
using XFrame.Modules.Diagnotics;
using UnityXFrame.Core.HotUpdate;

namespace Game.Core.Procedure
{
    public class MainProcedure : ProcedureBase
    {
        protected override void OnEnter()
        {
            base.OnEnter();
            ChangeState<CheckResUpdateProcedure>();
        }
    }
}
