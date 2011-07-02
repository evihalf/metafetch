using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace metafetch
{
    public sealed class AppLog
    {
        private static readonly AppLog m_instance = new AppLog();
        public StringBuilder m_log;

        public enum Severity
        {
            Debug,
            Information,
            Warning,
            Error,
            Critical
        }

        private AppLog()
        {
            m_log = new StringBuilder();
        }

        public void Log(Severity severity, string message)
        {
            string logMessage = "[" + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + "] " + severity.ToString() + ": " + message;

            m_log.AppendLine(logMessage);
            System.Diagnostics.Debug.WriteLine(logMessage);
        }

        public IEnumerable<string> LogMessages
        {
            get
            {
                return m_log.ToString().Split(new char[] {'\n'});
            }
        }

        public static AppLog Instance
        {
            get { return m_instance; }
        }


    }
}
