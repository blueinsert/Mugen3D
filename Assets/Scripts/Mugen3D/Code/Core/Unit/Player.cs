using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Mugen3D
{
    public enum PlayerId
    {
        P1,
        P2,
        P3,
        P4
    }
  
    [RequireComponent(typeof(Animation))]
    public class Player : Unit, IHealth
    {

        public PlayerId id;
       
        public override void Init()
        {
            base.Init();
            moveCtr = new PlayerMoveCtrl(this);
            moveCtr.SetGravity(0, -10, 0);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            cmdMgr.Update(InputHandler.GetInputKeycode(this.id, this.facing));
        }


        private int m_maxHP = 100;
        private int m_hp = 100;

        public int GetHP()
        {
            return m_hp;
        }

        public int GetMaxHP()
        {
            return m_maxHP;
        }

        public void AddHP(int hpAdd)
        {
            m_hp += hpAdd;
            if (m_hp <= 0)
            {
                SendEvent(new Event { type = EventType.Dead });
            }
        }

        public void SetHP(int hp)
        {
            m_hp = hp;
        }
    }
}
