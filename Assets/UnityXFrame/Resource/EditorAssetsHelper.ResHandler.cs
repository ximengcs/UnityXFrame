using System;
using XFrame.Modules.Resource;

namespace UnityXFrame.Core.Resource
{
    public partial class EditorAssetsHelper
    {
        private class ResHandler : IResHandler
        {
            private Type m_HandleType = typeof(ResLoadTask);

            public object Data { get; }

            public bool IsDone => true;

            public float Pro => 1;

            public Type HandleType => m_HandleType;

            public ResHandler(object res)
            {
                Data = res;
            }
        }
    }
}
