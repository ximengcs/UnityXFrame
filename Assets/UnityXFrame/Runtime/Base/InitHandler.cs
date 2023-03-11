﻿using HybridCLR;
using UnityEngine;
using XFrame.Core;
using System.Reflection;
using XFrame.Modules.Tasks;
using XFrame.Modules.XType;
using XFrame.Modules.Config;
using XFrame.Modules.Resource;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Download;
using System;

namespace UnityXFrame.Core
{
    public class InitHandler : IInitHandler
    {
        public void EnterHandle()
        {
            InitData data = Init.Inst.Data;
            XConfig.Lang = data.Language;
            XConfig.Entrance = data.Entrance;
            XConfig.DefaultRes = data.ResMode;
            XConfig.DefaultLogger = data.Logger;
            XConfig.ArchivePath = Constant.ArchivePath;
            XConfig.DefaultJsonSerializer = data.JsonSerializer;
            XConfig.DefaultDownloadHelper = data.DownloadHelper;
            if (data.LocalizeFile != null)
                XConfig.LocalizeFile = data.LocalizeFile.text;
        }

        public ITask BeforeHandle()
        {
            return InnerInitHotfixTask();
        }

        public ITask AfterHandle()
        {
            InnerConfigLog();
            return new EmptyTask();
        }

        private ITask InnerInitHotfixTask()
        {
            InitData data = Init.Inst.Data;
            XTask task = TaskModule.Inst.GetOrNew<XTask>();
            if (!string.IsNullOrEmpty(data.AOTMetaDllPath))
            {
                ResLoadTask<TextAsset> loadAOTTask = ResModule.Inst.LoadAsync<TextAsset>(data.AOTMetaDllPath);
                loadAOTTask.OnComplete((asset) =>
                {
                    byte[] bytes = asset.bytes;
                    if (bytes != null)
                        RuntimeApi.LoadMetadataForAOTAssembly(bytes, Init.Inst.Data.AOTMetaMode);
                });
                task.Add(loadAOTTask);
            }

            if (!string.IsNullOrEmpty(data.HotfixDllPath))
            {
                ResLoadTask<TextAsset> loadHotTask = ResModule.Inst.LoadAsync<TextAsset>(data.HotfixDllPath);
                loadHotTask.OnComplete((asset) =>
                {
                    byte[] bytes = asset.bytes;
                    if (bytes != null)
                    {
                        Assembly.Load(bytes);
                    }
                });
                task.Add(loadHotTask);
            }

            bool success = false;
            bool start = false;
            BolActionTask testTask = TaskModule.Inst.GetOrNew<BolActionTask>();
            testTask.Add(() =>
            {
                if (!start)
                {
                    start = true;
                    string url = "ftp://47.108.188.157/pub/test/Hotfix.bytes";
                    DownloadModule.Inst.DownData(url, (data) =>
                    {
                        Log.Debug("download success");
                        Log.Debug(data.Length);
                        AppDomain.CurrentDomain.AssemblyLoad += (sender, args) =>
                        {
                            Log.Debug(sender.GetType().Name);
                            Log.Debug(args.LoadedAssembly.FullName);
                        };
                        Assembly.Load(data);
                        success = true;
                    }, () =>
                    {
                        Log.Debug("download error");
                        success = true;
                    });
                }
                return success;
            });
            task.Add(testTask);

            //string path = Path.Combine(Application.persistentDataPath, "hotfix.bytes");
            //if (File.Exists(path))
            //{
            //    byte[] testbytes = File.ReadAllBytes(path);
            //    if (testbytes != null)
            //    {
            //        Assembly.Load(testbytes);
            //        TypeModule.Inst.UpdateType();
            //    }
            //}

            return task;
        }

        private void InnerConfigLog()
        {
            Diagnotics.Logger logger = LogModule.Inst.GetLogger<Diagnotics.Logger>();
            foreach (DebugColor colorData in Init.Inst.Data.LogMark)
            {
                if (colorData.Value)
                    logger.Register(colorData.Key, colorData.Color);
            }
        }
    }
}
