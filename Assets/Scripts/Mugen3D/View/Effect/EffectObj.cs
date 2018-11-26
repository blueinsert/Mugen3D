using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    public class EffectObj : MonoBehaviour
    {
        protected bool isAlive = true;
        public int id;
        public event Action<int> onFinish;

        public bool IsAlive() {
            return isAlive;
        }

        public virtual void Play() {
            isAlive = true;
        }

        protected void OnFinish()
        {
            if(onFinish != null)
            {
                onFinish(this.id);
            }
        }
    }
}
