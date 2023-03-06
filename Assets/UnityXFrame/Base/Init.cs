using UnityEngine;
using UnityXFrame.Core.Diagnotics;
using XFrame.Core;
using XFrame.Modules.Config;
using XFrame.Modules.Diagnotics;

namespace UnityXFrame.Core
{
    public class Init : SingletonMono<Init>
    {
        [SerializeField] public InitData Data;

        private void Awake()
        {
            InnerConfigData();
            Entry.Init();
            InnerConfigFrame();
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

        private void InnerConfigData()
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

        private void InnerConfigFrame()
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