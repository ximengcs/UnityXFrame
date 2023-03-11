
using System.IO;
using System.Net;

namespace XFrame.Modules.Download
{
    public class FtpDownloadHelper : IDownloadHelper
    {
        private FtpWebRequest m_Request;
        private FtpWebResponse m_Response;

        public bool IsDone { get; private set; }
        public DownloadResult Result { get; private set; }

        void IDownloadHelper.Request(string url)
        {
            m_Request = (FtpWebRequest)WebRequest.Create(url);
            m_Request.Method = WebRequestMethods.Ftp.DownloadFile;
            m_Response = (FtpWebResponse)m_Request.GetResponse();
            Stream responseStream = m_Response.GetResponseStream();
            BinaryReader reader = new BinaryReader(responseStream);
            
            Result = new DownloadResult(m_Response.ContentLength > 0,
                    null, reader?.ReadBytes((int)m_Response.ContentLength),
                    m_Response.StatusDescription);
            reader.Close();
            m_Response.Close();
            IsDone = true;
        }

        void IDownloadHelper.Update()
        {

        }

        void IDownloadHelper.Dispose()
        {
            m_Response?.Dispose();
            m_Request = null;
            IsDone = default;
            Result = default;
        }
    }
}
