using System.IO;
using UnityEngine;
using XFrame.Modules;
using System.Collections.Generic;

namespace UnityXFrame.Core
{
    public partial class AssetBundleResHelper : IResourceHelper
    {
        private AssetBundle m_Main;
        private AssetBundleManifest m_MainManifest;
        private FileNode<FileLoadInfo> m_BundleTree;
        private Dictionary<string, BundleInfo> m_Bundles;

        public AssetBundleResHelper()
        {
            m_BundleTree = new FileNode<FileLoadInfo>("Assets");
            m_Bundles = new Dictionary<string, BundleInfo>();
        }

        public XTask Init()
        {
            Dictionary<string, string[]> dependencies = new Dictionary<string, string[]>();
            XTask task = new XTask();
            XTask loadTask = TaskModule.Inst.GetOrNew(nameof(loadTask));

            task.Add(() =>
                {
                    m_Main = AssetBundle.LoadFromFile(Path.Combine(Application.persistentDataPath, "bundles"));
                    m_MainManifest = m_Main.LoadAsset<AssetBundleManifest>(nameof(AssetBundleManifest));
                    return true;
                })
                .Add(() =>
                {
                    if (!loadTask.IsStart)
                    {
                        foreach (string abName in m_MainManifest.GetAllAssetBundles())
                        {
                            string abFile = Path.Combine(Application.persistentDataPath, abName);
                            AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(abFile);
                            request.completed += (op) =>
                            {
                                AssetBundle ab = request.assetBundle;
                                BundleInfo info = new BundleInfo(abFile);
                                info.Bundle = ab;
                                m_Bundles.Add(abName, info);
                                dependencies.Add(abFile, m_MainManifest.GetAllDependencies(abName));
                            };
                            loadTask.Add(() => request.isDone);
                        }
                        loadTask.Start();
                    }

                    return loadTask.IsComplete;
                })
                .Add(() =>
                {
                    foreach (BundleInfo info in m_Bundles.Values)
                    {
                        string[] dependsNames = dependencies[info.Name];
                        BundleInfo[] dependAb = new BundleInfo[dependsNames.Length];
                        for (int i = 0; i < dependsNames.Length; i++)
                        {
                            dependAb[i] = m_Bundles[dependsNames[i]];
                        }
                        info.Dependencies = dependAb;

                        foreach (string resName in info.Bundle.GetAllAssetNames())
                        {
                            FileLoadInfo fileInfo = new FileLoadInfo(resName, info);
                            m_BundleTree.Add(fileInfo.NameWithoutExt, fileInfo);
                        }
                    }
                    m_Main.Unload(true);
                    return true;
                })
                .Start();

            return task;
        }

        public void LoadAsync(string dirName, string fileName, System.Action<Object> complete)
        {
            string path = InnerCheckFileName(dirName, fileName);
            if (m_BundleTree.TryGetFile(path, out FileLoadInfo loadInfo))
            {
                BundleInfo bundle = loadInfo.Bundle;
                bundle.LoadAsync(loadInfo.Name, complete);
            }
            else
                complete(default);
        }

        public object Load(string dirName, string fileName)
        {
            string path = InnerCheckFileName(dirName, fileName);
            if (m_BundleTree.TryGetFile(path, out FileLoadInfo loadInfo))
            {
                BundleInfo bundle = loadInfo.Bundle;
                return bundle.Load(loadInfo.Name);
            }
            else
                return default;
        }

        public object Load(params string[] namePart)
        {
            return Load(Path.Combine(namePart), string.Empty);
        }

        public void LoadAsync<T>(string dirName, string fileName, System.Action<T> complete) where T : Object
        {
            string path = InnerCheckFileName(dirName, fileName);
            if (m_BundleTree.TryGetFile(path, out FileLoadInfo loadInfo))
            {
                BundleInfo bundle = loadInfo.Bundle;
                bundle.LoadAsync(loadInfo.Name, complete);
            }
            else
                complete(default);
        }

        public T Load<T>(string dirName, string fileName)
        {
            string path = InnerCheckFileName(dirName, fileName);
            if (m_BundleTree.TryGetFile(path, out FileLoadInfo loadInfo))
            {
                BundleInfo bundle = loadInfo.Bundle;
                return (T)(object)bundle.Load(loadInfo.Name);
            }
            else
                return default;
        }

        public T Load<T>(params string[] namePart)
        {
            return (T)(object)Load(Path.Combine(namePart), string.Empty);
        }

        public void Unload(string package)
        {
            if (m_Bundles.TryGetValue(package, out BundleInfo bundleInfo))
                bundleInfo.Unload();
        }

        public void UnloadAll()
        {
            AssetBundle.UnloadAllAssetBundles(true);
        }

        private string InnerCheckFileName(string dirName, string fileName)
        {
            dirName = dirName.Replace('_', '\\');
            dirName = dirName.Replace('/', '\\');
            string path = Path.Combine(m_BundleTree.Name, dirName, fileName).ToLower();
            return Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path));
        }

        public void LoadAllAsync(System.Action complete)
        {
            AsyncOperateSet opSet = new AsyncOperateSet();
            opSet.OnComplete(complete);
            foreach (BundleInfo info in m_Bundles.Values)
                opSet.Add(info.Bundle.LoadAllAssetsAsync());
        }
    }
}
