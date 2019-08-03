using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bluebean.UGFramework
{
    public static class ClassLoader
    {
        public static Type GetType(string typeFullName)
        {
            var type = System.Reflection.Assembly.Load("Assembly-CSharp").GetType(typeFullName);
            #if UNITY_EDITOR
            if (type == null)
            {
                type = System.Reflection.Assembly.Load("Assembly-CSharp-Editor").GetType(typeFullName);
            }
            #endif
            return type;
        }

        public static object CreateInstance(string typeFullName, params object[] param)
        {
            var type = GetType(typeFullName);
            if (type == null)
                return null;
            var instance = Activator.CreateInstance(type, param);
            return instance;
        }
    }
}
