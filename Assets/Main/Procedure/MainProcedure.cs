using System;
using UnityEngine;
using XFrame.Modules.XType;
using XFrame.Modules.Tasks;
using XFrame.Modules.Download;
using XFrame.Modules.Resource;
using XFrame.Modules.Procedure;
using XFrame.Modules.Diagnotics;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityXFrame.Core.Resource;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Game.Core.Procedure
{
    public class MainProcedure : ProcedureBase
    {
        protected override void OnEnter()
        {
            base.OnEnter();

            HotUpdateTask updateTask = TaskModule.Inst.GetOrNew<HotUpdateTask>();
            updateTask.Start();

            string url = "ftp://47.108.188.157/pub/test/Hotfix.bytes";
            Log.Debug(url);
            DownTask downer = DownloadModule.Inst.Down(url);
            downer.OnComplete(() =>
            {
                if (downer.Success)
                {
                    byte[] data = downer.Data;
                    Log.Debug("download success");
                    Log.Debug(data.Length);
                    AppDomain.CurrentDomain.AssemblyLoad += (sender, args) =>
                    {
                        Log.Debug(sender.GetType().Name);
                        Log.Debug(args.LoadedAssembly.FullName);
                    };
                    TypeModule.Inst.LoadAssembly(data);
                }
                else
                {
                    Log.Debug("download error");
                }
                InnerTest1();
            }).Start();

            Log.Debug("Test", "I am test string.");
            Log.Debug("I am test string.");
            Log.Debug("None", "I am test string.");
            Log.Debug("Default", "I am test string.");


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
