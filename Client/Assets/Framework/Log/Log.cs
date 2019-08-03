using System;
using System.Text;
using UnityEngine;

namespace bluebean.UGFramework.Log
{
    public class Debug
    {
        public static void DrawRect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Color c, float duration)
        {
            UnityEngine.Debug.DrawLine(p1, p2, c, duration);
            UnityEngine.Debug.DrawLine(p2, p3, c, duration);
            UnityEngine.Debug.DrawLine(p3, p4, c, duration);
            UnityEngine.Debug.DrawLine(p4, p1, c, duration);
        }

        public static void Log(string message)
        {
            UnityEngine.Debug.Log(FormatLog(message));
        }

        public static void LogWarning(string message)
        {
            UnityEngine.Debug.LogWarning(FormatLog(message));
        }

        public static void LogError(string messages)
        {
            UnityEngine.Debug.LogError(FormatLog(messages));
        }

        public static void Binary(string message, byte[] data)
        {
            var sb = new StringBuilder();
            sb.Append(message);
            sb.Append("[");
            foreach (var b in data)
            {
                sb.Append(b);
                sb.Append(" ");
            }
            sb.Append("]");
            Log(sb.ToString());
        }

        public static void Assert(bool condition, object message)
        {
            if (!condition)
            {
                LogError("Assert Failed! " + message);
            }
        }

        private static string FormatLog(string message)
        {
            var now = DateTime.Now;
            var prefix = string.Format("[{0}.{1}] ", now.ToString("yyyy-MM-dd HH:mm:ss"), now.Millisecond);
            return prefix + message;
        }
    }
}
