﻿using XFrame.Core;
using XFrame.Modules.Tasks;
using XFrame.Modules.Config;
using XFrame.Modules.Diagnotics;
using UnityEngine;

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
            return new EmptyTask();
        }

        public ITask AfterHandle()
        {
            InnerConfigLog();
            return new EmptyTask();
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
