using System;
using XFrame.Core;
using XFrame.Modules.Resource;

namespace UnityXFrame.Core.Resource
{
    /// <summary>
    /// 本地资源加载 (Resources)
    /// </summary>
    public partial class NativeResModule : SingletonModule<NativeResModule>
    {
        private ResourcesHelper m_ResHelper;

        /// <summary>
        /// 初始化生命周期
        /// </summary>
        /// <param name="data">初始化数据</param>
        public override void OnInit(object data)
        {
            base.OnInit(data);
            m_ResHelper = new ResourcesHelper();
            m_ResHelper.OnInit(default);
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="resPath">
        ///     资源路径，可传递相对于Assets的路径或相对于Resources的路径，如：
        ///     Assets/Resources/test.png = Assets/Resources/test = test.png = test
        /// </param>
        /// <param name="type">资源类型</param>
        /// <returns>加载到的资源</returns>
        public UnityEngine.Object Load(string resPath, Type type)
        {
            return (UnityEngine.Object)m_ResHelper.Load(resPath, type);
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="resPath">
        ///     资源路径，可传递相对于Assets的路径或相对于Resources的路径，如：
        ///     Assets/Resources/test.png = Assets/Resources/test = test.png = test
        /// </param>
        /// <returns>加载到的资源</returns>
        public T Load<T>(string resPath) where T : UnityEngine.Object
        {
            return m_ResHelper.Load<T>(resPath);
        }

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <param name="resPath">
        ///     资源路径，可传递相对于Assets的路径或相对于Resources的路径，如：
        ///     Assets/Resources/test.png = Assets/Resources/test = test.png = test
        /// </param>
        /// <param name="type">资源类型</param>
        /// <returns>资源加载任务</returns>
        public ResLoadTask LoadAsync(string resPath, Type type)
        {
            return m_ResHelper.LoadAsync(resPath, type);
        }

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="resPath">
        ///     资源路径，可传递相对于Assets的路径或相对于Resources的路径，如：
        ///     Assets/Resources/test.png = Assets/Resources/test = test.png = test
        /// </param>
        /// <returns>资源加载任务</returns>
        public ResLoadTask<T> LoadAsync<T>(string resPath)
        {
            return m_ResHelper.LoadAsync<T>(resPath);
        }

        /// <summary>
        /// 卸载资源
        /// </summary>
        /// <param name="res">需要卸载的资源</param>
        public void Unload(UnityEngine.Object res)
        {
            m_ResHelper.Unload(res);
        }

        /// <summary>
        /// 卸载所有资源
        /// </summary>
        public void UnloadAll()
        {
            m_ResHelper.UnloadAll();
        }
    }
}
