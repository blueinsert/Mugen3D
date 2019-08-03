using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bluebean.UGFramework
{
    public class AutoBindAttribute : Attribute
    {
        public string path { get; private set; }

        public AutoBindAttribute(string path)
        {
            this.path = path;
        }
    }
}