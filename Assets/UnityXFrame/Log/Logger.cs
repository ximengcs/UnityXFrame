using UnityEngine;

namespace UnityXFrame.Core.Diagnotics
{
    public partial class Logger : XFrame.Modules.Diagnotics.ILogger
    {
        private bool m_MustRegister;
        private Formater m_Formater;

        public Logger()
        {
            m_MustRegister = false;
            m_Formater = new Formater();
        }

        public void Register(string name, Color color)
        {
            m_Formater.Register(name, color);
        }

        public void Debug(params object[] content)
        {
            if (InnerFormat(out string result, content) || !m_MustRegister)
                UnityEngine.Debug.Log(result);
        }

        public void Error(params object[] content)
        {
            if (InnerFormat(out string result, content) || !m_MustRegister)
                UnityEngine.Debug.LogError(result);
        }

        public void Fatal(params object[] content)
        {
            if (InnerFormat(out string result, content) || !m_MustRegister)
                UnityEngine.Debug.LogError(result);
        }

        public void Warning(params object[] content)
        {
            if (InnerFormat(out string result, content) || !m_MustRegister)
                UnityEngine.Debug.LogWarning(result);
        }

        private bool InnerFormat(out string result, params object[] content)
        {
            if (content.Length > 1)
            {
                if (m_Formater.Format(content[0].ToString(), content[1].ToString(), out result))
                    return true;
                else
                    return false;
            }
            else
            {
                result = string.Concat(content);
                return true;
            }
        }
    }
}
