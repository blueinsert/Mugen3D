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

        public void ExeController(Player p, StateEventType type, Dictionary<string,string> param){
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
            }
        }

        public void VelSet(Player p, Dictionary<string,string> param){
            float velx,vely;
            velx = float.Parse(param["x"]);
            vely = float.Parse(param["y"]);
            p.moveCtr.VelSet(velx, vely);

        }

        public void VelAdd(Player p, Dictionary<string, string> param)
        {
            float x, y;
            x = float.Parse(param["x"]);
            y = float.Parse(param["y"]);
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
            string anim = param["value"];
            p.animCtr.SetPlayAnim(anim);
        }

        public void PosSet(Player p, Dictionary<string, string> param)
        {
            float x, y;
            x = float.Parse(param["x"]);
            y = float.Parse(param["y"]);
            p.moveCtr.PosSet(x, y);
        }


        public void PosAdd(Player p, Dictionary<string, string> param)
        {
            float x, y;
            x = float.Parse(param["x"]);
            y = float.Parse(param["y"]);
            p.moveCtr.PosAdd(x, y);
        }

        public void PosFreeze(PlayerId id, Dictionary<string, string> param)
        {

        }
        #endregion
    }
}
