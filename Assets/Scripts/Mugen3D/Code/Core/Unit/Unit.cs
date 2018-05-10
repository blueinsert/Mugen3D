using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace Mugen3D
{
    [RequireComponent(typeof(DecisionBoxes))]
    public abstract class Unit : Entity
    {
        
        public AnimationController animCtr;
        public CmdManager cmdMgr;
        public FsmManager fsmMgr;
        public MoveCtrl moveCtr;
       
        public Status status = new Status();

        [HideInInspector]
        public int facing = 1;
        private int pauseTime = 0;
        public int teamId = 0;

        protected void Init(UnitConfig config)
        {
           
        }

        public override void OnUpdate()
        {
            if (IsPause())
            {
                pauseTime--;
                return;
            }
            fsmMgr.Update();
            moveCtr.Update();
            animCtr.Update();
        }
   
        public void ChangeFacing(int facing)
        {
            if (this.facing != facing)
            {
                this.facing = facing;
                var scale = this.transform.localScale;
                this.transform.localScale = new Vector3(scale.x, scale.y, Mathf.Abs(scale.z)*facing);
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
