using UnityEngine;
using XFrame.Core;
using XFrame.Modules.Config;
using XFrame.Modules.Diagnotics;
using UnityXFrame.Core.Resource;

namespace UnityXFrame.Core
{
    public class Init : MonoBehaviour
    {
        [SerializeField] public InitData m_Data;

        private void Awake()
        {
            InnerConfigData();
            Entry.Init();
            InnerConfigFrame();
        }

        private void Start()
        {
            Entry.Register<NativeResModule>();
            Entry.Register<Debuger>(m_Data.DebuggerSkin);
            Entry.Start();
        }

        private void Update()
        {
            Entry.Update(Time.deltaTime);
        }

        private void OnGUI()
        {
            Debuger.Inst.OnGUI();
        }

        private void OnDestroy()
        {
            Entry.ShutDown();
        }

        private void InnerConfigData()
        {
            XConfig.Lang = m_Data.Language;
            XConfig.DefaultRes = m_Data.ResMode;
            XConfig.DefaultLogger = m_Data.Logger;
            XConfig.ArchivePath = Constant.ArchivePath;
            XConfig.DefaultJsonSerializer = m_Data.JsonSerializer;
            if (m_Data.LocalizeFile != null)
                XConfig.LocalizeFile = m_Data.LocalizeFile.text;
        }

        private void InnerConfigFrame()
        {
            Diagnotics.Logger logger = LogModule.Inst.GetLogger<Diagnotics.Logger>();
            foreach (DebugColor colorData in m_Data.LogMark)
            {
                if (colorData.Value)
                    logger.Register(colorData.Key, colorData.Color);
            }
        }
    }
}