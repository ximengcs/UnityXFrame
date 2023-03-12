using XFrame.Modules.Tasks;
using XFrame.Modules.Diagnotics;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets.ResourceLocators;

namespace UnityXFrame.Core.Resource
{
    public class HotUpdateTask : TaskBase
    {
        protected override void OnInit()
        {
            AddStrategy(new Strategy());
            Add(new Handler());
        }

        private class Strategy : ITaskStrategy<Handler>
        {
            private Handler m_Handler;

            public void OnUse(Handler handler)
            {
                m_Handler = handler;
                m_Handler.Check();
            }

            public float OnHandle(ITask from)
            {
                switch (m_Handler.State)
                {
                    case State.WaitCheck:
                    case State.CheckFailure:
                        return MAX_PRO;

                    case State.CheckSuccess:
                        m_Handler.Download();
                        return 0;

                    case State.Downloading:
                        float pro = m_Handler.Pro;
                        if (pro >= MAX_PRO)
                            pro = 0.988888f;
                        return pro;

                    case State.DownloadSuccess:
                        return MAX_PRO;

                    case State.DownloadFailure:
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
            WaitCheck,
            Checking,
            CheckSuccess,
            CheckFailure,
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
            public bool HasUpdate { get; private set; }
            public float Pro { get; private set; }

            public Handler()
            {
                State = State.WaitCheck;
            }

            public void Check()
            {
                AsyncOperationHandle<List<string>> checkHandle = Addressables.CheckForCatalogUpdates(true);
                State = State.Checking;
                checkHandle.Completed += (handle) =>
                {
                    if (checkHandle.Status == AsyncOperationStatus.Succeeded)
                    {
                        State = State.CheckSuccess;
                        m_CheckList = checkHandle.Result;
                        m_Count = m_CheckList.Count;
                        HasUpdate = m_Count > 0;
                        m_ProRate = 1 / m_Count;
                        Log.Debug("XFrame", $"Check catalog success. list count:{m_Count}");
                    }
                    else
                    {
                        Log.Debug("XFrame", "Check catalog failure.");
                        State = State.CheckFailure;
                    }
                };
            }

            public void Download()
            {
                if (State != State.CheckSuccess)
                {
                    Log.Debug("XFrame", "Check failure, cant download res.");
                    State = State.DownloadFailure;
                }
                else
                {
                    AsyncOperationHandle<List<IResourceLocator>> updateHandle = Addressables.UpdateCatalogs(m_CheckList);
                    State = State.Downloading;
                    updateHandle.Completed += (handle) =>
                    {
                        if (handle.Status == AsyncOperationStatus.Succeeded)
                        {
                            Log.Debug("XFrame", "UpdateCatalogs success.");
                            List<IResourceLocator> list = updateHandle.Result;
                            foreach (IResourceLocator locator in list)
                            {
                                AsyncOperationHandle<long> sizeHandle = Addressables.GetDownloadSizeAsync(locator.Keys);
                                sizeHandle.Completed += (handle) =>
                                {
                                    if (handle.Status == AsyncOperationStatus.Succeeded)
                                    {
                                        long downSize = handle.Result;
                                        Log.Debug("XFrame", $"Down {locator.LocatorId} size : {downSize}");
                                        if (downSize > 0)
                                        {
                                            InnerStartDownload(locator);
                                        }
                                    }
                                    else
                                    {
                                        Log.Debug("XFrame", $"Get {locator.LocatorId} size failure, cant download res.");
                                        State = State.DownloadFailure;
                                    }
                                };
                            }
                        }
                        else
                        {
                            Log.Debug("XFrame", "UpdateCatalogs failure, cant download res.");
                            State = State.DownloadFailure;
                        }
                    };
                }
            }

            private void InnerStartDownload(IResourceLocator locator)
            {
                AsyncOperationHandle downHandle = Addressables.DownloadDependenciesAsync(locator.Keys);
                BolActionTask task = TaskModule.Inst.GetOrNew<BolActionTask>();
                float curPor = 0;
                task.Add(() =>
                {
                    bool isDone = downHandle.IsDone;
                    if (isDone)
                    {
                        if (downHandle.Status == AsyncOperationStatus.Succeeded)
                        {
                            Log.Debug("XFrame", $"Down {locator.LocatorId} success.");
                            m_Count--;
                            if (m_Count == 0)
                            {
                                State = State.DownloadSuccess;
                            }
                        }
                        else
                        {
                            State = State.DownloadFailure;
                            Log.Debug("XFrame", $"Down {locator.LocatorId} failure, cant download res.");
                        }
                    }
                    else
                    {
                        Pro -= curPor;
                        curPor = downHandle.PercentComplete * m_ProRate;
                        Pro += curPor;
                    }
                    return isDone;
                });
            }
        }
    }
}
