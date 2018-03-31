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

        public void ChangeAnim(Unit p, int animNo, string playMode = "Once")
        {
            p.ChangeAnim(animNo, playMode);
        }

        public void PosSet(Unit p, float x, float y)
        {
            p.moveCtr.PosSet(x, y);
        }

        public void PhysicsSet(Unit p, string physics)
        {
            switch (physics)
            {
                case "S":
                    p.status.physicsType = PhysicsType.S;
                    break;
                case "C":
                    p.status.physicsType = PhysicsType.C;
                    break;
                case "A":
                    p.status.physicsType = PhysicsType.A;
                    break;
                case "N":
                    p.status.physicsType = PhysicsType.N;
                    break;
                default:
                    p.status.physicsType = PhysicsType.S;
                    break;
            }
        }

        public void MoveTypeSet(Unit p, string moveType)
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

        public void Pause(Unit p, int pauseTime)
        {
            p.Pause(pauseTime);
        }

        #endregion
    }

}
