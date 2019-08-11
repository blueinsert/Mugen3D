using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace bluebean.Mugen3D.ClientGame
{
    public class UIView : MonoBehaviour
    {
        public Action<UIView> onDestroy;
        public int id { get; private set; }
        public UIDef def { get; private set; }
          
        public void Close() {
            if (onDestroy != null)
            {
                onDestroy(this);
            }
        }

        public virtual void Init(int id, UIDef def)
        {
            this.id = id;
            this.def = def;
        }
    }
}
