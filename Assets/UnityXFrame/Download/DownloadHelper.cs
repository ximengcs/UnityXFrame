using UnityEditor.PackageManager.Requests;
using UnityEngine.Networking;
using XFrame.Modules;

namespace UnityXFrame.Core
{
    public class DownloadHelper : IDownloadHelper
    {
        private UnityWebRequest m_Request;

        public bool IsDone { get; private set; }
        public DownloadResult Result { get; private set; }

        public void Request(string url)
        {
            m_Request = UnityWebRequest.Get(url);
            m_Request.SendWebRequest();
        }

        public void Update()
        {
            if (m_Request == null)
                return;

            if (!m_Request.isDone)
                return;

            IsDone = true;
            Result = new DownloadResult(m_Request.result == UnityWebRequest.Result.Success,
                    m_Request.downloadHandler.text,
                    m_Request.downloadHandler.data,
                    m_Request.error);
            m_Request = null;
        }

        public void Dispose()
        {
            m_Request.Dispose();
            m_Request = null;
            IsDone = default;
            Result = default;
        }
    }
}
