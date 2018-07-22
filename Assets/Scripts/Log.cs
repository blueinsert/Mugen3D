using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    public class Log
    {
        public static void Info(string info)
        {
            Debug.Log(info);
        }

        public static void Warn(string info)
        {
            Debug.LogWarning(info);
        }

        public static void Error(string info)
        {
            Debug.LogError(info);
        }

        public static void DrawLine(Vector2 start, Vector2 end, Color c, float duration)
        {
            Debug.DrawLine(start, end, c, duration);
        }

        public static void DrawRect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Color c, float duration)
        {
            Debug.DrawLine(p1, p2, c, duration);
            Debug.DrawLine(p2, p3, c, duration);
            Debug.DrawLine(p3, p4, c, duration);
            Debug.DrawLine(p4, p1, c, duration);
        }
    }
}
