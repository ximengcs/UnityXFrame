using System;
using System.Reflection;
using XFrame.Modules.Tasks;
using XFrame.Modules.Resource;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityXFrame.Core.Resource
{
    public partial class AddressablesHelper : IResourceHelper
    {
        private Dictionary<int, ResHandler> m_LoadMap;

        void IResourceHelper.OnInit(string rootPath)
        {
            m_LoadMap = new Dictionary<int, ResHandler>();
        }

        public object Load(string resPath, Type type)
        {
            throw new NotImplementedException();
        }

        public T Load<T>(string resPath)
        {
            throw new NotImplementedException();
        }

        public ResLoadTask LoadAsync(string resPath, Type type)
        {
            throw new NotImplementedException();
        }

        public ResLoadTask<T> LoadAsync<T>(string resPath)
        {
            ResLoadTask<T> loadTask = TaskModule.Inst.GetOrNew<ResLoadTask<T>>();
            AsyncOperationHandle handle = Addressables.LoadAssetAsync<T>(resPath);
            ResHandler handler = new ResHandler(handle);
            loadTask.OnComplete((asset) =>
            {
                int code = asset.GetHashCode();
                if (!m_LoadMap.ContainsKey(code))
                    m_LoadMap.Add(code, handler);
            });
            loadTask.Add(handler);
            return loadTask;
        }

        public void Unload(object target)
        {
            int code = target.GetHashCode();
            if (m_LoadMap.TryGetValue(code, out ResHandler handler))
            {
                handler.Release();
                m_LoadMap.Remove(code);
            }
        }

        public void UnloadAll()
        {
            foreach (ResHandler handler in m_LoadMap.Values)
                handler.Release();
            m_LoadMap.Clear();
        }
    }
}
