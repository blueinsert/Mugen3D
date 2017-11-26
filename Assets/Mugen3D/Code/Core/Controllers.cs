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

        public void ExeController(Player p, StateEventType type, Dictionary<string,string> param, Action cb){
            switch(type){
                case StateEventType.VelSet:
                    VelSet(p, param);
                    break;
                case StateEventType.ChangeAnim:
                    ChangeAnim(p, param);
                    break;
                case StateEventType.ChangeState:
                    ChangeState(p, param);
                    break;
                case StateEventType.PosSet:
                    PosSet(p, param);
                    break;
                case StateEventType.PhysicsSet:
                    PhysicsSet(p, param);
                    break;
                case StateEventType.VarSet:
                    VarSet(p, param);
                    break;
                case StateEventType.HitDef:
                    HitDef(p, param, cb);
                    break;
                case StateEventType.Pause:
                    Pause(p,param);
                    break;
            }
        }

        public void VelSet(Player p, Dictionary<string,string> param){
            Log.Info("velSet");
            float x,y;
            if (param.ContainsKey("x"))
            {
                x = float.Parse(param["x"]);
            }
            else
            {
                x = Triggers.Instance.VelX(p);
            }
            if (param.ContainsKey("y"))
            {
                y = float.Parse(param["y"]);
            }
            else
            {
                y = Triggers.Instance.VelY(p);
            }
            p.moveCtr.VelSet(x * p.facing, y);

        }

        public void VelAdd(Player p, Dictionary<string, string> param)
        {
            float x, y;
            if (param.ContainsKey("x"))
            {
                x = float.Parse(param["x"]);
            }
            else
            {
                x = 0;
            }
            if (param.ContainsKey("y"))
            {
                y = float.Parse(param["y"]);
            }
            else
            {
                y = 0;
            }
            p.moveCtr.VelAdd(x, y);
        }

        public void CtrlSet(Player p, Dictionary<string, string> param)
        {
            bool value = !(int.Parse(param["value"]) == 0);
            p.canCtrl = value;
        }

        public void ChangeState(Player p, Dictionary<string, string> param)
        {
            int value = int.Parse(param["value"]);
            p.stateMgr.ChangeState(value);
        }

        public void ChangeAnim(Player p, Dictionary<string, string> param)
        {
            int animNo = int.Parse(param["value"]);
            
            if (param.ContainsKey("mode"))
            {
                string mode = param["mode"];
                if (mode == "loop")
                {
                    p.animCtr.PlayAnim(animNo, AnimPlayMode.Loop);
                }
                else if (mode == "once")
                {
                    p.animCtr.PlayAnim(animNo, AnimPlayMode.Once);
                }
                else
                {
                    Debug.LogError("anim mode can't be recognized:" + mode);
                }
            }
            else
            {
                p.animCtr.PlayAnim(animNo, AnimPlayMode.Loop);
            }
            
        }

        public void PosSet(Player p, Dictionary<string, string> param)
        {
            float x, y;
            if (param.ContainsKey("x"))
            {
                x = float.Parse(param["x"]);
            }
            else
            {
                x = Triggers.Instance.PosX(p);
            }
            if (param.ContainsKey("y"))
            {
                y = float.Parse(param["y"]);
            }
            else
            {
                y = Triggers.Instance.PosY(p);
            }
            p.moveCtr.PosSet(x, y);
        }


        public void PosAdd(Player p, Dictionary<string, string> param)
        {
            float x, y;
            if (param.ContainsKey("x"))
            {
                x = float.Parse(param["x"]);
            }
            else
            {
                x = 0;
            }
            if (param.ContainsKey("y"))
            {
                y = float.Parse(param["y"]);
            }
            else
            {
                y = 0;
            }
            p.moveCtr.PosAdd(x, y);
        }

        public void PosFreeze(Player p, Dictionary<string, string> param)
        {

        }

        public void PhysicsSet(Player p, Dictionary<string, string> param)
        {
            string v = param["value"];
            switch (v)
            {
                case "S":
                    p.moveCtr.SetPhysicsType(PhysicsType.Stand);
                    break;
                case "C":
                    p.moveCtr.SetPhysicsType(PhysicsType.Croch);
                    break;
                case "A":
                    p.moveCtr.SetPhysicsType(PhysicsType.Air);
                    break;
                default:
                    p.moveCtr.SetPhysicsType(PhysicsType.Stand);
                    break;
            }
        }

        public void VarSet(Player p, Dictionary<string, string> param)
        {
            int id = int.Parse(param["id"]);
            int value = int.Parse(param["value"]);
            p.SetVar(id, value);
        }

        public void Pause(Player p, Dictionary<string, string> param)
        {
            int pauseTime;
            pauseTime = int.Parse(param["time"]);
            p.Pause(pauseTime);
        }

        private bool IsHit(Player p, HitBoxLocation activePart, Player enemy)
        {
            HitBox attackBox = p.GetComponent<DecisionBoxManager>().GetHitBox(activePart);
            HitBox[] attackBoxes = new HitBox[] { attackBox};
            DefenceBox[] defenceBoxes = enemy.GetComponent<DecisionBoxManager>().defenceBoxes.ToArray();
            bool hit = false;
            for (int i = 0; i < attackBoxes.Length; i++)
            {
                for (int j = 0; j < defenceBoxes.Length; j++)
                {
                    if (ColliderSystem.CuboidCuboidTest(attackBoxes[i].cuboid.GetVertexArray().ToArray(), defenceBoxes[j].cuboid.GetVertexArray().ToArray()))
                    {
                        hit = true;
                        break;
                    }
                    if (hit == true)
                        break;
                }
            }
            Log.Info("hit:" + hit);
            return hit;
        }

        public void HitDef(Player p, Dictionary<string, string> param, Action cb)
        {
            
           Player enemy = TeamMgr.GetEnemy(p);
           if (enemy == null)
               return;
           HitVars hitvars = new HitVars(param);
           bool hit = IsHit(p, HitBoxLocation.Hand_L, enemy);
           if (!hit)
               return;
           cb();
           p.Pause(hitvars.p1HitPauseTime);
           enemy.SetHitVars(hitvars);
           //change state
           enemy.stateMgr.ChangeState(5000);
        }

        #endregion
    }

}
