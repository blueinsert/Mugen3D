using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    public class Controllers
    {
        private Controllers() { }

        private static Controllers mInstance;
        public static Controllers Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new Controllers();
                    mInstance.Init();
                }
                return mInstance;
            }
        }

        private void Init()
        {

        }

        #region controller function

      
        public void DestroySelf(Unit p)
        {
            p.Destroy();
        }

        /*
        public void CreateHelper(Unit p, Dictionary<string, TokenList> param)
        {
            Mugen3D.EntityLoader.LoadHelper(param["name"].asStr, p as Player, p.transform.parent, param);
        }
         */
        
        public void VelSet(Unit p, float x, float y)
        {
            p.moveCtr.VelSet(x * p.facing, y);
        }

        public void VelAdd(Player p, float x, float y)
        {
            p.moveCtr.VelAdd(x, y);
        }

        public void CtrlSet(Unit p, bool isCtrl)
        {
            p.status.ctrl = isCtrl;
        }

        public void ChangeAnim(Unit p, string animName, string playMode)
        {
            p.animCtr.ChangeAnim(animName, playMode);
        }

        public void PosSet(Unit p, float x, float y)
        {
            p.moveCtr.PosSet(x, y);
        }

        public void SetPhysics(Unit p, string physics)
        {
            switch (physics)
            {
                case "S":
                    p.status.physicsType = PhysicsType.Stand;
                    break;
                case "C":
                    p.status.physicsType = PhysicsType.Crouch;
                    break;
                case "A":
                    p.status.physicsType = PhysicsType.Air;
                    break;
                case "N":
                    p.status.physicsType = PhysicsType.None;
                    break;
                default:
                    p.status.physicsType = PhysicsType.Stand;
                    break;
            }
        }

        public void Pause(Unit p, int pauseTime)
        {
            p.Pause(pauseTime);
        }

        /*
        private bool IsHit(Unit p, HitPart activePart, Player enemy)
        {
            
            //HitBox attackBox = p.GetComponent<DecisionBoxManager>().GetHitBox(activePart);
            //HitBox[] attackBoxes = new HitBox[] { attackBox};
            //DefenceBox[] defenceBoxes = enemy.GetComponent<DecisionBoxManager>().defenceBoxes.ToArray();
            bool hit = false;
            for (int i = 0; i < attackBoxes.Length; i++)
            {
                for (int j = 0; j < defenceBoxes.Length; j++)
                {
                    if (PhysicsUtils.CuboidCuboidTest(attackBoxes[i].cuboid.GetVertexArray().ToArray(), defenceBoxes[j].cuboid.GetVertexArray().ToArray()))
                    {
                        hit = true;
                        break;
                    }
                    if (hit == true)
                        break;
                }
            }
            
            //Log.Info("hit:" + hit);
            return false;
        }

        public void HitDef(Unit p, Dictionary<string, TokenList> param)
        {
            Player enemy = p.enemy as Player;
           if (enemy == null)
               return;
           HitVars hitvars = new HitVars(param);
           bool hit = false;//IsHit(p, hitvars.activeAttackBodyPart, enemy);
           if (!hit)
               return;
           if (Triggers.Instance.EnemyMoveType(p) != "Defence")
           {
               p.Pause(hitvars.p1HitPauseTime);
               enemy.SetHitVars(hitvars);
               enemy.AddHP(-hitvars.hitDamage);
               //change state
               enemy.stateMgr.ChangeState(5000 + ((int)enemy.status.moveType) * 10);
           }
           else
           {
               p.Pause(hitvars.p1GuardPauseTime);
               enemy.SetHitVars(hitvars);
               enemy.AddHP(-hitvars.guardDamage);
               //change state
               int stateNo = 150 + ((int)enemy.status.moveType) * 2;
               Debug.Log("stateNo:" + stateNo);
               enemy.stateMgr.ChangeState(stateNo);
           }   
        }
*/

        public void SetMoveType(Unit p, string moveType)
        {
            switch (moveType)
            {
                case "A":
                    p.status.moveType = MoveType.Attack; break;
                case "I":
                    p.status.moveType = MoveType.Idle; break;
                case "D":
                    p.status.moveType = MoveType.Defence; break;
                case "H":
                    p.status.moveType = MoveType.BeingHitted; break;
                default:
                    p.status.moveType = MoveType.Idle; break;
            }
        }

        #endregion
    }

}
