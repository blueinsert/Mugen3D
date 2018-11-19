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

        public Character(string characterName, CharacterConfig config, int slot, bool isLocal) : base(config)
        {
            this.characterName = characterName;
            this.slot = slot;
            this.isLocal = isLocal;       
            cmdMgr = new CmdManager(config.commandContent, this);
            moveCtr = new CharacterMoveCtrl(this);
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

        public void CreateHelper(string name)
        {
            var helper = EntityFactory.CreateHelper(name, this);
            this.world.AddEntity(helper);
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
                this.fsmMgr.ChangeState(5000);
            }
            else if (hitDef.hitType == (int)HitDef.HitType.Throw)
            {
                this.fsmMgr.ChangeState(hitDef.p2StateNo);
            }   
        }

        public override void OnGuardHit(HitDef hitDef)
        {
            SetBeHitDefData(hitDef);
            this.fsmMgr.ChangeState(this.GetPhysicsType() == PhysicsType.S ? 150 : 156);
        }

        public override void OnMoveHit(Unit target)
        {
            var hitDef = GetHitDefData();
            hitDef.moveContact = true;
            hitDef.moveGuarded = false;
            hitDef.moveHit = true;
            hitDef.target = target;
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
            var hitDef = GetHitDefData();
            hitDef.moveContact = true;
            hitDef.moveGuarded = true;
            hitDef.moveHit = false;
            hitDef.target = target;
            CornerPushTest(target, true);
        }

        private void CornerPushTest(Unit target, bool moveGuarded)
        {
            var hitDef = GetHitDefData();
            if (target is Character && target.GetBackEdgeDist() < new Number(5) / new Number(10))
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
