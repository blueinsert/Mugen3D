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
        P4,
    }

    [RequireComponent(typeof(Animation))]
    public class Player : Unit, IHealth
    {
        const int randomSeed = 123456789;
        public System.Random randomGenerater = new System.Random(randomSeed);

        [HideInInspector]
        public PlayerId id;
       
        public override void Init()
        {
            base.Init();
            //
            moveCtr = new PlayerMoveCtrl(this);
            //
            cmdMgr = new CmdManager();
            cmdMgr.SetOwner(this);
            cmdMgr.LoadCmdFile(commandFile);
            //
            animCtr = new AnimationController(this.GetComponent<Animation>(), this, animDefFile);
            //
            stateMgr = new StateManager(this);
            stateMgr.ReadStateDefFile(stateFiles.ToArray());
            //
            config = new Config(configFile);

            moveCtr.SetGravity(0, config.GetConfig("Gravity"), 0);
            vars = new Dictionary<int, int>();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            moveCtr.Update();
            cmdMgr.Update(InputHandler.GetInputKeycode(this.id, this.facing));
            animCtr.Update();
            stateMgr.Update();
        }

        public void Reset()
        {
            SetHP(GetMaxHP());
            this.stateMgr.ChangeState(0);
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
