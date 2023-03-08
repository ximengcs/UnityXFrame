using UnityEngine;
using XFrame.Core;
using XFrame.Modules.Config;
using XFrame.Modules.Diagnotics;
using UnityXFrame.Core.Diagnotics;

namespace UnityXFrame.Core
{
    public class Init : SingletonMono<Init>
    {
        public InitData Data;

        private void Awake()
        {
            InnerConfig();
            InnerInitBefore();
            Entry.Init();
            InnerInitAfter();
        }

        private void Start()
        {
            Entry.Start();
        }

        private void Update()
        {
            Entry.Update(Time.deltaTime);
        }

        private void OnGUI()
        {
#if CONSOLE
            Debuger.Inst.OnGUI();
#endif
        }

        private void OnDestroy()
        {
            Entry.ShutDown();
        }

        private void InnerConfig()
        {
            XConfig.Lang = Data.Language;
            XConfig.Entrance = Data.Entrance;
            XConfig.DefaultRes = Data.ResMode;
            XConfig.DefaultLogger = Data.Logger;
            XConfig.ArchivePath = Constant.ArchivePath;
            XConfig.DefaultJsonSerializer = Data.JsonSerializer;
            if (Data.LocalizeFile != null)
                XConfig.LocalizeFile = Data.LocalizeFile.text;
        }

        private void InnerInitBefore()
        {

        }

        private void InnerInitAfter()
        {
            Diagnotics.Logger logger = LogModule.Inst.GetLogger<Diagnotics.Logger>();
            foreach (DebugColor colorData in Data.LogMark)
            {
                if (colorData.Value)
                    logger.Register(colorData.Key, colorData.Color);
            }
        }
    }
}