using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Mugen3D
{
    [RequireComponent(typeof(Animation))]
    public class Character : Unit, IHealth
    {

        public int slot;
        public string characterName;

        public void Init(string characterName, CharacterConfig config)
        {
            base.Init(config);
            this.characterName = characterName;
            moveCtr = new PlayerMoveCtrl(this);
            string prefix = "Chars/" + characterName;
            ActionsConfig actionsConfig = ConfigReader.Read<ActionsConfig>(ResourceLoader.LoadText(prefix + config.actionConfigFile));
            animCtr = new AnimationController(actionsConfig, this.GetComponent<Animation>(), this);
            cmdMgr = new CmdManager(ResourceLoader.LoadText(prefix + config.cmdConfigFile), this);
            fsmMgr = new FsmManager(prefix + config.fsmConfigFile, this);
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
