using UnityEngine;
using XFrame.Modules.Resource;
using XFrame.Modules.Procedure;
using XFrame.Modules.Diagnotics;

namespace Game.Core.Procedure
{
    public class MainProcedure : ProcedureBase
    {
        protected override void OnEnter()
        {
            base.OnEnter();

            Log.Debug("Test", "I am test string.");
            Log.Debug("I am test string.");
            Log.Debug("None", "I am test string.");
            Log.Debug("Default", "I am test string.");
            //ITask task = ResModule.Inst.Preload(
            //    new string[] { "Assets/Data/Sprites/test.png" },
            //    new Type[] { typeof(Sprite) });
            //task.OnComplete(InnerTest).Start();
        }

        private void InnerTest()
        {
            Sprite sprite = ResModule.Inst.Load<Sprite>("Assets/Data/Sprites/test.png");
            GameObject inst = new GameObject();
            inst.AddComponent<SpriteRenderer>().sprite = sprite;
        }
    }
}
