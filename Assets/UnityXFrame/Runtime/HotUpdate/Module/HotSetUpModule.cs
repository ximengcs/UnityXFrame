using System;
using XFrame.Core;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Procedure;
using XFrame.Modules.XType;

namespace UnityXFrame.Core.HotUpdate
{
    [XModule]
    public class HotSetUpModule : SingletonModule<HotSetUpModule>
    {
        protected override void OnStart()
        {
            base.OnStart();
            TypeModule.Inst.OnTypeChange(InnerTypeHandler);
        }

        public void StartHotProc()
        {
            ProcedureModule.Inst.Redirect(Constant.HOTFIX_ENTRANCE);
        }

        private void InnerTypeHandler()
        {
            Entry.AddModules<HotModuleAttribute>();
            TypeSystem typeSys = TypeModule.Inst.GetOrNew<HotProcedureBase>();
            foreach (Type type in typeSys)
            {
                Log.Debug(type.Name);
                ProcedureModule.Inst.Add(type);
            }
            StartHotProc();
        }
    }
}
