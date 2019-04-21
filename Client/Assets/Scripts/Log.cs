using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Mugen3D
{
    public class Log
    {
        public static void DrawRect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Color c, float duration)
        {
            Debug.DrawLine(p1, p2, c, duration);
            Debug.DrawLine(p2, p3, c, duration);
            Debug.DrawLine(p3, p4, c, duration);
            Debug.DrawLine(p4, p1, c, duration);
        }

        public static void Info(string message)
        {
            Debug.Log(FormatLog(message));
        }

        public static void Warn(string message)
        {
            Debug.LogWarning(FormatLog(message));
        }

        public static void Error(string messages)
        {
            Debug.LogError(FormatLog(messages));
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
            Info(sb.ToString());
        }

        public static void Assert(bool condition, object message)
        {
            if (!condition)
            {
                Error("Assert Failed! " + message);
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
