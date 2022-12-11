using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityXFrame.Core
{
    public class AsyncOperateSet
    {
        private int m_Count;
        private List<AsyncOperation> m_List;
        private Action m_Complete;

        public AsyncOperateSet()
        {
            m_List = new List<AsyncOperation>();
        }

        public AsyncOperateSet Add(AsyncOperation op)
        {
            m_List.Add(op);
            m_Count++;
            op.completed += InnerOpComplete;
            return this;
        }

        public void OnComplete(Action complete)
        {
            m_Complete = complete;
        }

        private void InnerOpComplete(AsyncOperation op)
        {
            m_Count--;
            if (m_Count <= 0)
            {
                m_Complete?.Invoke();
                m_Complete = null;
                m_List = null;
            }
        }
    }
}
