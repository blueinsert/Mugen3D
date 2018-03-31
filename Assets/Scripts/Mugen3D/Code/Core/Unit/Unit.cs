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
        public TextAsset commandFile;
        public TextAsset animMappingFile;
      
        public DecisionBoxes decisionBoxes;

        public XLua.LuaTable fsm;
        private delegate void DegateFsmInit(XLua.LuaTable self, Unit u);
        private delegate void DelegateFsmUpdate(XLua.LuaTable self);
        private delegate void DelegateFsmChangeState(XLua.LuaTable self, int stateNo);
        private DegateFsmInit funFsmInit;
        private DelegateFsmUpdate funcFsmUpdate;
        private DelegateFsmChangeState funFsmChangeState;

        private AnimationsDef m_animMapping;
        public AnimationController animCtr;
        public CmdManager cmdMgr;
        public MoveCtrl moveCtr;

        public Status status = new Status();

        [HideInInspector]
        public int facing = 1;
        private int pauseTime = 0;
        public int teamId = 0;

        public override void Init()
        {
            decisionBoxes = this.GetComponent<DecisionBoxes>();
            decisionBoxes.SetOwner(this);
            decisionBoxes.Init();
            if (animMappingFile != null)
                m_animMapping = new AnimationsDef(animMappingFile.text);
            animCtr = new AnimationController(this.GetComponent<Animation>(), this);
            cmdMgr = new CmdManager();
            cmdMgr.SetOwner(this);
            cmdMgr.LoadCmdFile(commandFile);
            //
        }

        public void SetFSM(XLua.LuaTable fsm)
        {
            this.fsm = fsm;
            funcFsmUpdate = this.fsm.Get<string, DelegateFsmUpdate>("update");
            funFsmChangeState = this.fsm.Get<string, DelegateFsmChangeState>("changeState");     
        }

        public override Collider GetCollider()
        {
            return decisionBoxes.GetCollideBox();
        }

        public override void OnUpdate()
        {
            if (IsPause())
            {
                pauseTime--;
                return;
            }
            if (funcFsmUpdate != null)
            {
                funcFsmUpdate(this.fsm);
            }
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
            if (funFsmChangeState != null)
            {
                funFsmChangeState(fsm, stateNo);
            }
        }

        public void ChangeAnim(int animNo, string playMode = "Once")
        {
            Debug.Log("ChangeAnim");
            this.status.animNo = animNo;
            string animName = m_animMapping.GetAnimName(animNo);
            Debug.Log("animName:" + animName);
            this.animCtr.ChangeAnim(animName, playMode);
        }

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
    }
}
