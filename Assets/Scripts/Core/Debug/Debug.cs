using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D.Core
{
    public delegate void LogDelegate(string message);
    public delegate void AssertDelegate(bool expr, object message);
    public delegate void AddGUIDebugMsgDelegate(int slot, string key, string value);

    public class Debug
    {
        public static LogDelegate Log;
        public static LogDelegate LogWarn;
        public static LogDelegate LogError;
        public static AssertDelegate Assert;
        public static AddGUIDebugMsgDelegate AddGUIDebugMsg;
    }

}
