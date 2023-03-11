using UnityEngine.Networking;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Download;

namespace UnityXFrame.Core.Download
{
    public class DownloadHelper : IDownloadHelper
    {
        private UnityWebRequest m_Request;
        private bool m_Complete;

        public bool IsDone { get; private set; }
        public DownloadResult Result { get; private set; }

        void IDownloadHelper.Request(string url)
        {
            m_Complete = false;
            m_Request = UnityWebRequest.Get(url);
            m_Request.SendWebRequest();
        }

        void IDownloadHelper.Update()
        {
            if (m_Complete)
                return;

            if (m_Request == null)
                return;

            if (!m_Request.isDone)
                return;

            IsDone = true;
            Result = new DownloadResult(m_Request.result == UnityWebRequest.Result.Success,
                    m_Request.downloadHandler.text,
                    m_Request.downloadHandler.data,
                    m_Request.error);

            m_Complete = true;
        }

        void IDownloadHelper.Dispose()
        {
            m_Request?.Dispose();
            m_Request = null;
            IsDone = default;
            Result = default;
        }
    }
}
