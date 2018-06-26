using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    public class Log
    {
        public static void Info(string info)
        {
            Debug.Log(World.Instance.gameTime + ":" + info);
        }

        public static void Warn(string info)
        {
            Debug.LogWarning(World.Instance.gameTime + ":" + info);
        }

        public static void Error(string info)
        {
            Debug.LogError(World.Instance.gameTime + ":" + info);
        }

        public static void DrawLine(Vector2 start, Vector2 end, Color c, float duration)
        {
            Debug.DrawLine(start, end, c, duration);
        }
    }
}
