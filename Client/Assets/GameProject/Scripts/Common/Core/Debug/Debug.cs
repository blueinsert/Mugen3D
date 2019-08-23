using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bluebean.Mugen3D.Core
{
    public delegate void LogDelegate(string message);
    public delegate void AssertDelegate(bool expr, object message);

    public class Debug
    {
        public static LogDelegate m_Log;
        public static LogDelegate m_LogWarn;
        public static LogDelegate m_LogError;
        public static AssertDelegate m_Assert;

        public static void Log(string msg)
        {
            if (m_Log != null)
            {
                m_Log(msg);
            }
        }

        public static void LogError(string msg)
        {
            if (m_LogError != null)
            {
                m_LogError(msg);
            }
        }

        public static void LogWarn(string msg)
        {
            if (m_LogWarn != null)
            {
                m_LogWarn(msg);
            }
        }
    }

}
