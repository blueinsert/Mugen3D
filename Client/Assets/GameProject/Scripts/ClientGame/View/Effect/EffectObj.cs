using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bluebean.Mugen3D.ClientGame
{
    public class EffectObj : MonoBehaviour
    {
        public int id { get; private set; }
        public event Action<int> onFinish;
        private Core.EffectDef m_def;
        private int m_lifeTime = 0;
        private UnitView m_owner;
        private bool m_isDestroied = false;
        private bool m_isInited = false;

        public virtual void Init(int id, Core.EffectDef def, UnitView owner)
        {
            this.id = id;
            m_def = def;
            m_lifeTime = 0;
            m_owner = owner;
            this.transform.localScale = new Vector3(def.facing, 1, 1);
            m_isDestroied = false;
            m_isInited = true;
        }

        public virtual bool IsAlive()
        {
            if (m_def.removeTime > 0 && m_lifeTime >= m_def.removeTime)
            {
                return false;
            }
            return true;
        }

        public virtual void Update()
        {
            if (!m_isInited)
                return;
            if (m_isDestroied)
                return;
            m_lifeTime++;
            if(!IsAlive())
            {
                Destroy();
                return;
            }
            if(m_lifeTime <= m_def.bindTime)
            {
                BindToTarget();
            }
        }

        private void BindToTarget()
        {
            Vector3 pos = Vector3.zero;
            switch (m_def.posType)
            {
                case "p1":
                    var p = m_def.pos.AsVector3();
                    p.x *= m_owner.unit.GetFacing();
                    pos = m_owner.transform.position + p; ;
                    break;
            }
            this.transform.position = pos;
        }

        public virtual void Play() {
        }

        protected void Destroy()
        {
            m_isDestroied = true;
            if(onFinish != null)
            {
                onFinish(this.id);
            }
        }
    }
}
