using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D.Core
{
    public delegate void LogDelegate(string message, params object[] args);
    public delegate void AssertDelegate(bool expr, object message);

    public class Debug
    {
        public static LogDelegate Log;
        public static LogDelegate LogWarn;
        public static LogDelegate LogError;
        public static AssertDelegate Assert;
    }

}
