using System.Collections;
using System.Collections.Generic;
using Vector = Mugen3D.Core.Vector;
using Number = Mugen3D.Core.Number;

namespace Mugen3D.Core
{
    public class Character : Unit
    {
        public string characterName { get; private set; }
        public int slot { get; private set; }
        public bool isLocal { get; private set; }
        public CmdManager cmdMgr { get; protected set; }
        private int input;
        private int m_aiLevel = 0;
        public int AILevel {get {return m_aiLevel;}}
        private List<Helper> m_helpers = new List<Helper>();
        private List<Projectile> m_projs = new List<Projectile>();
        public int roundsExisted { get; private set; }
        private bool m_canAttack = true;

        public Character(string characterName, CharacterConfig config, int slot, bool isLocal) : base(config)
        {
            this.characterName = characterName;
            this.slot = slot;
            this.isLocal = isLocal;       
            cmdMgr = new CmdManager(config.commandContent, this);
            moveCtr = new CharacterMoveCtrl(this);
        }

        public bool CanAttack()
        {
            return m_canAttack;
        }

        public bool SetCanAttack(bool value)
        {
            return m_canAttack = value;
        }

        public override void OnUpdate(Number deltaTime)
        {
            base.OnUpdate(deltaTime);
            cmdMgr.Update(input);
        }

        public void UpdateInput(int input)
        {
            this.input = input;
        }

        public int NumProj()
        {
            return this.m_projs.Count;
        }

        public int NumProj(int id)
        {
            int count = 0;
            foreach(var proj in this.m_projs)
            {
                if(proj.projDef.id == id)
                {
                    count++;
                }
            }
            return count;
        }

        public int NumHelper()
        {
            return m_helpers.Count;
        }

        public void RemoveHelper(Helper h)
        {
            this.m_helpers.Remove(h);
        }

        public void RemoveProj(Projectile proj)
        {
            this.m_projs.Remove(proj);
        }

        public void CreateHelper(string name)
        {
            var helper = EntityFactory.CreateHelper(name, this);
            this.world.AddEntity(helper);
            this.m_helpers.Add(helper);
        }

        public void CreateProjectile(string name, ProjectileDef def)
        {
            var projectile = EntityFactory.CreateProjectile(name, def, this);
            this.world.AddEntity(projectile);
            this.m_projs.Add(projectile);
        }

        public void SetAILevel(int aiLevel) {
            this.m_aiLevel = aiLevel;
        }

        public override bool IsGuarding()
        {
            return fsmMgr.stateNo >= 120 && fsmMgr.stateNo < 150;
        }

        public override void OnBeHitted(HitDef hitDef)
        {
            SetBeHitDefData(hitDef);
            if (hitDef.hitType == (int)HitDef.HitType.Attack)
            {
                AddHP(-hitDef.hitDamage);
                if (!IsAlive() && hitDef.knockAwayType == -1)
                {
                    hitDef.knockAwayType = 0;
                }
                SendEvent(new Event() { type = EventType.PlayEffect, data = EffectDef.ConstructNormal(hitDef.spark, hitDef.sparkPos, this.GetFacing())});
                SendEvent(new Event() { type = EventType.PlaySound, data = new SoundDef() { name = hitDef.hitSound, delay = 0, volume = 1 } });
                if (this.GetMoveType() == MoveType.BeingHitted)
                {
                    AddBeHitCount();
                    hitDef.owner.SetHitCount(this.beHitCount);
                }
                else
                {
                    ClearBeHitCount();
                    AddBeHitCount();
                }
                if (hitDef.knockAwayType == -1) {
                    if (this.GetPhysicsType() == PhysicsType.S)
                    {
                        this.fsmMgr.ChangeState(5000, true);
                    }
                    else if (this.GetPhysicsType() == PhysicsType.C)
                    {
                        this.fsmMgr.ChangeState(5010, true);
                    }
                    else if (this.GetPhysicsType() == PhysicsType.A)
                    {
                        this.fsmMgr.ChangeState(5020, true);
                    }
                }
                else
                {
                    this.fsmMgr.ChangeState(5030, true);
                } 
            }
            else if (hitDef.hitType == (int)HitDef.HitType.Throw)
            {
                this.fsmMgr.ChangeState(hitDef.p2StateNo, true);
            }   
        }

        public override void OnGuardHit(HitDef hitDef)
        {
            SetBeHitDefData(hitDef);
            this.fsmMgr.ChangeState(this.GetPhysicsType() == PhysicsType.S ? 150 : 156);
            AddHP(-hitDef.guardDamage);
            SendEvent(new Event() { type = EventType.PlayEffect, data = EffectDef.ConstructNormal(hitDef.guardSpark, hitDef.sparkPos, this.GetFacing()) });
            SendEvent(new Event() { type = EventType.PlaySound, data = new SoundDef() { name = hitDef.guardSound, delay = 0, volume = 1 } });
        }

        public override void OnMoveHit(Unit target)
        {
            base.OnMoveHit(target);
            var hitDef = GetHitDefData();
            if (hitDef.hitType == (int)HitDef.HitType.Attack)
            {
                CornerPushTest(target, false);   
            }
            else if (hitDef.hitType == (int)HitDef.HitType.Throw)
            {
                this.fsmMgr.ChangeState(hitDef.p1StateNo);
            }  
        }

        public override void OnMoveGuarded(Unit target)
        {
            base.OnMoveGuarded(target);
            CornerPushTest(target, true);
        }

        private void CornerPushTest(Unit target, bool moveGuarded)
        {
            var hitDef = GetHitDefData();
            if (target is Character && target.GetBackStageDist() < new Number(5) / new Number(10))
            {
                Number velX = 0;
                if (moveGuarded)
                    velX = hitDef.guardVel.X();
                else if (target.GetPhysicsType() == PhysicsType.A)
                    velX = hitDef.airVel.X();
                else
                    velX = hitDef.groundVel.X();
                if (this.GetPhysicsType() == PhysicsType.A)
                    this.moveCtr.VelAdd(-Number.Abs(velX) * hitDef.airCornerPush, 0);
                else
                    this.moveCtr.VelAdd(-Number.Abs(velX) * hitDef.groundCornerPush, 0);
            }
        }
 
        public override void PrintDebugInfo()
        {
            base.PrintDebugInfo();
            Core.Debug.AddGUIDebugMsg(this.id, "command", this.cmdMgr.GetActiveCommandName());
        } 
    }

}
