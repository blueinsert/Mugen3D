﻿using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    public class UIView : MonoBehaviour
    {
        public Action<UIView> onDestroy;
        public int id { get; private set; }
        public new string name { get; private set; }
          
        public void Close() {
            if (onDestroy != null)
            {
                onDestroy(this);
            }
        }

        public virtual void Init(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}
