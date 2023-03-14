using System;
using UnityEngine;
using XFrame.Modules.Tasks;
using XFrame.Modules.Resource;
using XFrame.Modules.Procedure;
using XFrame.Modules.Diagnotics;
using UnityXFrame.Core.Resource;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Game.Core.Procedure
{
    public class MainProcedure : ProcedureBase
    {
        protected override void OnEnter()
        {
            base.OnEnter();

            Log.Debug("Generate hot update check task.");
            HotUpdateCheckTask checkTask = TaskModule.Inst.GetOrNew<HotUpdateCheckTask>();
            Log.Debug("Start hot update check task.");
            checkTask.OnComplete(() =>
                {
                    if (checkTask.Success)
                    {
                        List<string> list = checkTask.ResList;
                        Log.Debug($"Hot update check task has success. require down count : {list.Count}");
                        foreach (string item in list)
                            Log.Debug($"Item : {item}");
                        if (list.Count > 0)
                        {
                            Log.Debug("Generate hot update download task.");
                            HotUpdateDownTask downTask = TaskModule.Inst.GetOrNew<HotUpdateDownTask>();
                            Log.Debug("Start hot update download task.");
                            downTask.AddList(list)
                                .OnComplete(() =>
                                {
                                    if (downTask.Success)
                                    {
                                        Log.Debug("Hot update download task has success.");
                                        ChangeState<EnterHotfixProcedure>();
                                    }
                                    else
                                    {
                                        Log.Debug("Hot update download task has failure.");
                                    }
                                }).Start();
                        }
                        else
                        {
                            ChangeState<EnterHotfixProcedure>();
                            Log.Debug($"Count is 0, dnt require download");
                        }
                    }
                    else
                    {
                        Log.Debug("Hot update check task has failure.");
                    }
                }).Start();
        }

        private void InnerTest1()
        {
            AsyncOperationHandle<List<string>> op = Addressables.CheckForCatalogUpdates(true);
            op.Completed += (op) =>
            {
                List<string> list = op.Result;
                foreach (string a in list)
                    Log.Debug("item " + a);

                if (list.Count > 0)
                {
                    var op2 = Addressables.UpdateCatalogs(list);
                    op2.Completed += (op2) =>
                    {

                    };
                }
            };
            ITask task = ResModule.Inst.Preload(
           new string[] { "Sprites/test.png" },
           new Type[] { typeof(Sprite) });
            task.OnComplete(InnerTest).Start();
        }

        private void InnerTest()
        {
            Debug.LogWarning("InnerTest");
            Sprite sprite = ResModule.Inst.Load<Sprite>("Sprites/test.png");
            GameObject inst = new GameObject();
            inst.AddComponent<SpriteRenderer>().sprite = sprite;
        }
    }
}
