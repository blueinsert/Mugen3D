using System.Collections;
using System.Collections.Generic;
using Vector = Mugen3D.Core.Vector;
using Number = Mugen3D.Core.Number;

namespace Mugen3D.Core
{
    public class Character : Unit, IHealth
    {
        public int slot;
        public string characterName;
        public CharacterConfig config;

        public Character(string characterName, CharacterConfig config)
        {
            this.characterName = characterName;
            this.config = config;
            moveCtr = new PlayerMoveCtrl(this);
            string prefix = "Chars/" + characterName;
            animCtr = new AnimationController(config.actions, this);
            cmdMgr = new CmdManager(config.commandContent, this);
            fsmMgr = new FsmManager(prefix + config.fsmConfigFile, this);
        }

        public void SetSlot(int slot)
        {
            this.slot = slot;
        }

        public override void OnUpdate(Number deltaTime)
        {
            base.OnUpdate(deltaTime);
            //
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
