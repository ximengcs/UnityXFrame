using XFrame.Core;
using UnityEngine;
using XFrame.Modules.Archives;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Download;
using XFrame.Modules.Resource;
using XFrame.Modules.Serialize;

namespace UnityXFrame.Core
{
    public class Init : MonoBehaviour
    {
        public GUISkin DebugSkin;

        void Awake()
        {
            Entry.Init();

        }

        void Start()
        {
            Entry.Start();
            Debuger.SetModule(Entry.Register<Debuger.Module>(DebugSkin));
        }

        void Update()
        {
            Entry.Update(Time.deltaTime);
        }

        void OnDestroy()
        {
            Entry.ShutDown();
        }
    }
}