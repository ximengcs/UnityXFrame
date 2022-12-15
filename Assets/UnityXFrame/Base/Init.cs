using System.IO;
using UnityEngine;
using XFrame.Core;
using XFrame.Modules.Local;
using XFrame.Modules.Config;
using UnityXFrame.Core.Resource;

namespace UnityXFrame.Core
{
    public class Init : MonoBehaviour
    {
        [SerializeField] public InitData m_Data;

        private void Awake()
        {
            XConfig.Lang = m_Data.Language;
            XConfig.DefaultRes = m_Data.ResMode;
            XConfig.DefaultLogger = m_Data.Logger;
            XConfig.ArchivePath = Constant.ArchivePath;
            XConfig.DefaultJsonSerializer = m_Data.JsonSerializer;
            if (m_Data.LocalizeFile != null)
                XConfig.LocalizeFile = m_Data.LocalizeFile.text;

            Entry.Init();
        }

        private void Start()
        {
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
    }
}