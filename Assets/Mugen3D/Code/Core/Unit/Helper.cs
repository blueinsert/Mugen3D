using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    public class Helper : Unit
    {
        [HideInInspector]
        public Player master;

        private Dictionary<string, TokenList> m_initParams;

        public void SetInitParams(Dictionary<string, TokenList> initParams)
        {
            m_initParams = initParams;
        }

        public override void Init()
        {
            moveCtr = new MoveCtr(this);
            //
            //cmdMgr = new CmdManager();
            //cmdMgr.LoadCmdFile(commandFile);
            //
            animCtr = new AnimationController(this.GetComponent<Animation>(), this, animDefFile);
            stateMgr = new StateManager(this);
            stateMgr.ReadStateDefFile(stateFiles.ToArray());
            //
            config = new Config(configFile);
            vars = new Dictionary<int, int>();
        }

        public override void OnUpdate()
        {
            moveCtr.Update();
            //if (m_lockInput == false)
            //    cmdMgr.Update(InputHandler.GetInputKeycode(this.id, this.facing));
            animCtr.Update();
            stateMgr.Update();
        }
    }
}
