using UnityEngine;
using XFrame.Core;
using XFrame.Modules;

namespace Assets.Scripts
{
    public class MainProcedure : ProcedureBase
    {
        protected override void OnEnter()
        {
            base.OnEnter();

            ResModule.Inst.SetResPath(Application.persistentDataPath);
            //Sprite test = ResModule.Inst.Load<Sprite>("Assets/Data/Sprites/test.png");
            
            ResModule.Inst.LoadAsync<Sprite>("Assets/Data/Sprites/test.png")
                .OnComplete((res) =>
                {
                    GameObject obj = new GameObject();
                    obj.AddComponent<SpriteRenderer>().sprite = res;
                });
        }
    }
}
