using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    public class Controllers
    {

        // Use this for initialization
        private Dictionary<PlayerId, Player> mPlayers;

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

        public void AddPlayer(PlayerId id, Player p)
        {
            mPlayers[id] = p;
        }

        #region controller function

        public void ExeController(PlayerId id, StateEventType type, Dictionary<string,string> param){
            switch(type){
                case StateEventType.VelSet:
                    VelSet(id, param);
                    break;
                case StateEventType.ChangeAnim:
                    ChangeAnim(id, param);
                    break;
                case StateEventType.ChangeState:
                    ChangeState(id, param);
                    break;
            }
        }

        public void VelSet(PlayerId id, Dictionary<string,string> param){

        }

        public void VelAdd(PlayerId id, Dictionary<string, string> param)
        {

        }

        public void CtrlSet(PlayerId id, Dictionary<string, string> param)
        {

        }

        public void ChangeState(PlayerId id, Dictionary<string, string> param)
        {

        }

        public void ChangeAnim(PlayerId id, Dictionary<string, string> param)
        {

        }

        public void PosSet(PlayerId id, Dictionary<string, string> param)
        {

        }

        public void PosAdd(PlayerId id, Dictionary<string, string> param)
        {

        }

        public void PosFreeze(PlayerId id, Dictionary<string, string> param)
        {

        }
        #endregion
    }
}
