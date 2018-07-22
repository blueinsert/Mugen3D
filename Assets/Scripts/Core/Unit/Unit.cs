using System;
using System.Collections;
using System.Collections.Generic;
using XLua;
using Math = Mugen3D.Core.Math;
using Vector = Mugen3D.Core.Vector;
using Number = Mugen3D.Core.Number;

namespace Mugen3D.Core
{
    public enum MoveType
    {
        Attack,
        Idle,
        Defence,
        BeingHitted,
    }

    public enum PhysicsType
    {
        N = -1,
        S,
        C,
        A,
    }

    public class Status
    {
        public MoveType moveType = MoveType.Idle;
        public PhysicsType physicsType = PhysicsType.S;
        public bool pushTest = true;
        public bool ctrl = true;
        public int animNo = -1;
    }

    public abstract class Unit : Entity
    {
        
        public AnimationController animCtr;
        public CmdManager cmdMgr;
        public FsmManager fsmMgr;
        public MoveCtrl moveCtr;
       
        public Status status = new Status();

        public int facing = 1;
        private int pauseTime = 0;
        private uint input;

        public Unit(UnitConfig cfg) : base(cfg)
        {
            this.cfg = cfg;
        }

        public override void OnUpdate(Number deltaTime)
        {
            if (IsPause())
            {
                pauseTime--;
                return;
            }
            fsmMgr.Update();
            moveCtr.Update(deltaTime);
            animCtr.Update();
            cmdMgr.Update(input);
        }

        public void UpdateInput(uint input)
        {
            this.input = input;
        }
   
        public void ChangeFacing(int facing)
        {
            if (this.facing != facing)
            {
                this.facing = facing;
                this.scale = new Vector(Math.Abs(scale.x)*facing, scale.y);
            }
        }

        public bool IsPause()
        {
            return pauseTime > 0;
        }

        public void Pause(int duration)
        {
            pauseTime = duration;
        }

        public void ChangeState(int stateNo, System.Action onExit)
        {
            fsmMgr.ChangeState(stateNo);
        }

        public void ChangeAnim(int animNo)
        {
           
            this.animCtr.ChangeAnim(animNo);
        }

        /*
        public bool IsHitOthers(HitPart activePart, out Unit hitTarget)
        {
            hitTarget = null;
            HitBox attackBox = this.decisionBoxes.GetHitBox(activePart);
            HitBox[] attackBoxes = new HitBox[] { attackBox};
            bool hit = false;
            foreach (var e in World.Instance.entities)
            {
                if (!(e is Unit))
                    continue;
                if ((e is Unit) && ((e as Unit).teamId == this.teamId))
                    continue;
                var u = e as Unit;
                Collider[] defenceBoxes = u.decisionBoxes.defenceBoxes.ToArray();
                for (int i = 0; i < attackBoxes.Length; i++)
                {
                    for (int j = 0; j < defenceBoxes.Length; j++)
                    {
                        if (PhysicsUtils.GeometryOverlapTest(attackBoxes[i].collider.GetGeometry(), defenceBoxes[j].GetGeometry()))
                        {
                            hit = true;
                            hitTarget = u;
                            break;
                        }                   
                    }
                    if (hit == true)
                        break;
                }
                if (hit == true)
                    break;
            }
            Debug.Log("ishit:" + hit);
            return hit;
        }
         */
    }
}
