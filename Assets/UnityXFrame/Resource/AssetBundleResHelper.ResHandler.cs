using System;
using UnityEngine;
using XFrame.Modules.Resource;

namespace UnityXFrame.Core.Resource
{
    public partial class AssetBundleResHelper
    {
        private class ResHandler : IResHandler
        {
            private Type m_HandleType = typeof(ResLoadTask);
            private AssetBundleRequest m_Request;

            public object Data => m_Request.asset;

            public bool IsDone => m_Request.isDone;

            public Type HandleType => m_HandleType;

            public float Pro => m_Request.progress;

            public ResHandler(AssetBundleRequest request)
            {
                m_Request = request;
            }
        }
    }
}
