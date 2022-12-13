using UnityEngine;
using UnityXFrame.Core;
using XFrame.Core;
using XFrame.Modules;

public class Init : MonoBehaviour
{
    public GUISkin DebugSkin;

    void Awake()
    {
        Entry.Init();
        LogModule.Inst.Register<UnityXFrame.Core.Logger>();
        SerializeModule.Inst.Register<JsonSerializeHelper>();
        ArchiveModule.Inst.SetPath(Application.persistentDataPath);
        ResModule.Inst.SetHelper<AssetBundleResHelper>();
    }

    void Start()
    {
        Entry.Start();
        Debuger.SetModule(Entry.Register<Debuger.Module>(DebugSkin));
        DownloadModule.Inst.Register<DownloadHelper>();
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
