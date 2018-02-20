using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    public class Helper : Unit
    {
        public Player master {
            get
            {
                return m_master;
            }
            set
            {
                m_master = value;
            }
        }

        private Player m_master;

        private Dictionary<string, TokenList> m_initParams;


        public void SetInitParams(Dictionary<string, TokenList> initParams)
        {
            m_initParams = initParams;   
        }

        public override void Init()
        {
            base.Init();

            moveCtr = new HelperMoveCtrl(this);
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

            InitStatus();
        }

        private void InitStatus()
        {
            if (m_initParams.ContainsKey("lifeTime"))
            {
                config.AddConfig("lifeTime".GetHashCode(), int.Parse(m_initParams["lifeTime"].asStr));
            }
            //pos
            string posType = "p1";
            if (m_initParams.ContainsKey("posType"))
            {
                posType = m_initParams["posType"].asStr;
            }
            Vector2 pos = Vector2.zero;
            if (m_initParams.ContainsKey("pos"))
            { 
                pos.x = float.Parse(m_initParams["pos"].tokens[0].value);
                pos.y = float.Parse(m_initParams["pos"].tokens[2].value);
            }
            this.transform.position = this.master.transform.position + new Vector3(pos.x, pos.y, 0);
            //stateNo
            int startStateNo = 0;
            if (m_initParams.ContainsKey("startStateNo"))
            {
                startStateNo = int.Parse(m_initParams["startStateNo"].asStr);
            }
            this.stateMgr.ChangeState(startStateNo);
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
