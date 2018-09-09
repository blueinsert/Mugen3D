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
        public bool isLocal;

        public Character(string characterName, CharacterConfig config, int slot, bool isLocal)
        {
            this.characterName = characterName;
            this.config = config;
            this.slot = slot;
            this.isLocal = isLocal;
            this.scale = new Vector(config.scaleX, config.scaleY, config.scaleZ);
            moveCtr = new PlayerMoveCtrl(this);
            string prefix = "Chars/" + characterName;
            animCtr = new AnimationController(config.actions, this);
            cmdMgr = new CmdManager(config.commandContent, this);
            fsmMgr = new FsmManager(prefix + config.fsmConfigFile, this);
        }

        public override void OnUpdate(Number deltaTime)
        {
            base.OnUpdate(deltaTime);
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


        public int AttackCheck()
        {
            if (this.animCtr.m_actions.ContainsKey(this.animCtr.anim))
            {
                var curAction = this.animCtr.m_actions[this.animCtr.anim];
                if (curAction.frames.Count != 0)
                {
                    var curActionFrame = curAction.frames[this.animCtr.animElem];
                    foreach (var clsn in curActionFrame.clsns)
                    {
                        if (clsn.type == 2)
                        {
                            Vector center = new Vector((clsn.x1 + clsn.x2) / 2, (clsn.y1 + clsn.y2) / 2, 0);
                            center.x = center.x * this.facing;
                            center += this.position;
                            Core.Rect rect = new Core.Rect(center, Math.Abs(clsn.x1 - clsn.x2), Math.Abs(clsn.y1 - clsn.y2));
                            return this.world.OverlapBox(rect, (character) => { return character != this; });
                        }
                    }
                }
            }
            return -1;
        }
    }

}
