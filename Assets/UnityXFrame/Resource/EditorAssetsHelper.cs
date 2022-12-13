//#if UNITY_EDITOR
//using System;
//using System.IO;
//using UnityEditor;
//using XFrame.Modules;

//namespace UnityXFrame.Core
//{
//    public class EditorAssetsHelper : IResourceHelper
//    {
//        private FileNode<object> m_ResCache;

//        public XTask Init()
//        {
//            m_ResCache = new FileNode<object>(string.Empty);
//            return XTask.Empty;
//        }

//        public object Load(string dirName, string fileName)
//        {
//            throw new System.NotSupportedException();
//        }

//        public object Load(params string[] namePart)
//        {
//            throw new System.NotSupportedException();
//        }

//        public T Load<T>(string dirName, string fileName)
//        {
//            string file = Path.Combine("assets", dirName, fileName);
//            if (m_ResCache.TryGetFile(file, out object res))
//            {
//                return (T)res;
//            }
//            else
//            {
//                Type type = typeof(T);
//                T resInst = (T)(object)AssetDatabase.LoadAssetAtPath(file, type);
//                m_ResCache.Add(file, resInst);
//                return resInst;
//            }
//        }

//        public T Load<T>(params string[] namePart)
//        {
//            return (T)Load(Path.Combine(namePart));
//        }

//        public void LoadAllAsync(System.Action complete)
//        {
//            complete();
//        }

//        public void Unload(string package)
//        {
//            m_ResCache.Remove(package);
//        }

//        public void UnloadAll()
//        {
//            m_ResCache.Clear();
//        }
//    }
//}
//#endif