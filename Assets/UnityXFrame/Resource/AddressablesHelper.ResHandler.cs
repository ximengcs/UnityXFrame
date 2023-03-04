using XFrame.Modules.Resource;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;
using UnityEngine.AddressableAssets;

namespace UnityXFrame.Core.Resource
{
    public partial class AddressablesHelper
    {
        private class ResHandler : IResHandler
        {
            private Action<object> m_OnComplete;
            private AsyncOperationHandle m_Handle;

            public object Data => m_Handle.Result;

            public bool IsDone => m_Handle.IsDone;

            public float Pro => m_Handle.PercentComplete;

            public ResHandler(AsyncOperationHandle handle)
            {
                m_Handle = handle;
            }

            public void Start()
            {
                InnerStart();
            }

            public void Release()
            {
                Addressables.Release(m_Handle);
            }

            public void OnComplete(Action<object> callback)
            {
                m_OnComplete = callback;
            }

            private async void InnerStart()
            {
                await m_Handle.Task;
                m_OnComplete?.Invoke(m_Handle.Result);
            }
        }
    }
}
