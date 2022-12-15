using System;
using UnityEngine;
using XFrame.Modules.Resource;

namespace UnityXFrame.Core.Resource
{
    public partial class NativeResModule
    {
        private class ResHandler : IResHandler
        {
            private ResourceRequest m_Request;
            private Type m_HandleType = typeof(ResLoadTask);

            public object Data => m_Request.asset;

            public bool IsDone => m_Request.isDone;

            public float Pro => m_Request.progress;

            public Type HandleType => m_HandleType;

            public ResHandler(ResourceRequest request)
            {
                m_Request = request;
            }
        }
    }
}
