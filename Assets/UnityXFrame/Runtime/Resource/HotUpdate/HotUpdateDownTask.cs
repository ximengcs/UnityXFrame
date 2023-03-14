using XFrame.Modules.Tasks;
using XFrame.Modules.Diagnotics;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityXFrame.Core.Resource
{
    public class HotUpdateDownTask : TaskBase
    {
        public bool Success { get; private set; }

        protected override void OnInit()
        {
            AddStrategy(new Strategy());
        }

        public HotUpdateDownTask AddList(List<string> downList)
        {
            Add(new Handler(downList));
            return this;
        }

        private class Strategy : ITaskStrategy<Handler>
        {
            private Handler m_Handler;

            public void OnUse(Handler handler)
            {
                m_Handler = handler;
                m_Handler.Download();
            }

            public float OnHandle(ITask from)
            {
                switch (m_Handler.State)
                {
                    case State.Downloading:
                        float pro = m_Handler.Pro;
                        if (pro >= MAX_PRO)
                            pro = 0.988888f;
                        return pro;

                    case State.DownloadSuccess:
                        HotUpdateDownTask task = (HotUpdateDownTask)from;
                        task.Success = true;
                        return MAX_PRO;

                    case State.DownloadFailure:
                        task = (HotUpdateDownTask)from;
                        task.Success = false;
                        return MAX_PRO;

                    default: return MAX_PRO;
                }
            }

            public void OnFinish()
            {
                m_Handler = null;
            }
        }

        private enum State
        {
            Downloading,
            DownloadFailure,
            DownloadSuccess
        }

        private class Handler : ITaskHandler
        {
            private List<string> m_CheckList;
            private float m_ProRate;
            private int m_Count;

            public State State { get; private set; }
            public float Pro { get; private set; }

            public Handler(List<string> downList)
            {
                m_CheckList = downList;
            }

            public void Download()
            {
                AsyncOperationHandle<List<IResourceLocator>> updateHandle = Addressables.UpdateCatalogs(m_CheckList);
                State = State.Downloading;
                updateHandle.Completed += (handle) =>
                {
                    if (handle.Status == AsyncOperationStatus.Succeeded)
                    {
                        Log.Debug("XFrame", "UpdateCatalogs success.");
                        List<IResourceLocator> list = updateHandle.Result;
                        List<object> keys = new List<object>();
                        foreach (IResourceLocator locator in list)
                            keys.AddRange(locator.Keys);

                        m_Count = keys.Count;
                        m_ProRate = 1f / m_Count;
                        foreach (object key in keys)
                        {
                            InnerCheckSize(key);
                        }
                    }
                    else
                    {
                        Log.Debug("XFrame", "UpdateCatalogs failure, cant download res.");
                        State = State.DownloadFailure;
                    }
                };
            }

            private void InnerCheckSize(object key)
            {
                AsyncOperationHandle<long> sizeHandle = Addressables.GetDownloadSizeAsync(key);
                sizeHandle.Completed += (handle) =>
                {
                    if (handle.Status == AsyncOperationStatus.Succeeded)
                    {
                        long downSize = handle.Result;
                        Log.Debug("XFrame", $"Down {key} {downSize}");
                        InnerDownload(key);
                    }
                    else
                    {
                        Log.Debug("XFrame", $"Get {key} size failure, cant download res.");
                        State = State.DownloadFailure;
                    }
                };
            }

            private void InnerDownload(object key)
            {
                AsyncOperationHandle downHandle = Addressables.DownloadDependenciesAsync(key);
                BolActionTask task = TaskModule.Inst.GetOrNew<BolActionTask>();
                float curPor = 0;
                task.Add(() =>
                {
                    bool isDone = downHandle.IsDone;
                    if (isDone)
                    {
                        if (downHandle.Status == AsyncOperationStatus.Succeeded)
                        {
                            Log.Debug("XFrame", $"Down {key} success.");
                            InnerCheckNextEnd();
                        }
                        else
                        {
                            State = State.DownloadFailure;
                            Log.Debug("XFrame", $"Down {key} failure, cant download res.");
                        }
                    }
                    else
                    {
                        Pro -= curPor;
                        curPor = downHandle.PercentComplete * m_ProRate;
                        Pro += curPor;
                    }
                    return isDone;
                }).Start();
            }

            private void InnerCheckNextEnd()
            {
                m_Count--;
                if (m_Count == 0)
                {
                    State = State.DownloadSuccess;
                    Pro = 1f;
                }
            }
        }
    }
}
