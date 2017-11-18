using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    public class Log
    {
        public static void Info(string info)
        {
            Debug.Log(GameEngine.gameTime + ":" + info);
        }

        public static void Warn(string info)
        {
            Debug.LogWarning(GameEngine.gameTime + ":" + info);
        }
    }
}
