using UnityEngine;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Procedure;
using XFrame.Modules.Resource;

namespace UnityXFrame.Core.Procedure
{
    public class MainProcedure : ProcedureBase
    {
        protected override void OnEnter()
        {
            base.OnEnter();

            ResModule.Inst.LoadAsync<Sprite>("Assets/Data/Sprites/test.png")
                .OnComplete((sprite) =>
                {
                    GameObject obj = new GameObject();
                    SpriteRenderer render = obj.AddComponent<SpriteRenderer>();
                    render.sprite = sprite;
                });
        }
    }
}
