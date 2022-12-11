using System.IO;
using UnityEngine;
using UnityXFrame.Core;
using XFrame.Modules;

namespace XFrame.Core
{
    public class ResourcesHelper : IResourceHelper
    {
        private FileNode<object> m_ResCache;

        public XTask Init()
        {
            m_ResCache = new FileNode<object>(string.Empty);
            return XTask.Empty;
        }

        public object Load(string dirName, string fileName)
        {
            string file = Path.Combine(dirName, fileName);
            file = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file));
            if (m_ResCache.TryGetFile(file, out object obj))
            {
                return obj;
            }
            else
            {
                Object res = Resources.Load(file);
                m_ResCache.Add(file, res);
                return res;
            }
        }

        public object Load(params string[] namePart)
        {
            return Load(Path.Combine(namePart));
        }

        public T Load<T>(string dirName, string fileName)
        {
            string file = Path.Combine(dirName, fileName);
            file = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file));
            if (m_ResCache.TryGetFile(file, out object obj))
            {
                return (T)obj;
            }
            else
            {
                T res = (T)(object)Resources.Load(file, typeof(T));
                m_ResCache.Add(file, res);
                return res;
            }
        }

        public T Load<T>(params string[] namePart)
        {
            return Load<T>(Path.Combine(namePart));
        }

        public void LoadAllAsync(System.Action complete)
        {
            throw new System.NotImplementedException();
        }

        public void Unload(string package)
        {
            if (m_ResCache.TryGetFile(package, out object resInst))
            {
                Resources.UnloadAsset((Object)resInst);
                m_ResCache.Remove(package);
            }
        }

        public void UnloadAll()
        {
            Resources.UnloadUnusedAssets();
            m_ResCache.Clear();
        }
    }
}
