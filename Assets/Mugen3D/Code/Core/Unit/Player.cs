using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Mugen3D
{
    [RequireComponent(typeof(Animation))]
    [RequireComponent(typeof(DecisionBoxManager))]
    public class Player : Unit, Collideable
    {
        const int randomSeed = 123456789;
        public System.Random randomGenerater = new System.Random(randomSeed);

        [HideInInspector]
        public PlayerId id;
        [HideInInspector]
        public bool canCtrl = true;

        private bool m_lockInput = false;

        public override void Init()
        {
            //
            moveCtr = new MoveCtr(this);
            //
            cmdMgr = new CmdManager();
            cmdMgr.LoadCmdFile(commandFile);
            //
            animCtr = new AnimationController(this.GetComponent<Animation>(), this, animDefFile);
            //
            stateMgr = new StateManager(this);
            stateMgr.ReadStateDefFile(stateFiles.ToArray());
            //
            config = new Config(configFile);

            moveCtr.gravity = new Vector3(0, config.GetConfig("Gravity"), 0);

            vars = new Dictionary<int, int>();
        }

        /*
        public void ChangeFacing(int facing)
        {
            if (this.facing != facing)
            {
                this.facing = facing;
                this.transform.localScale = new Vector3(1, 1, facing);
            }
        }*/

        public override void OnUpdate()
        {
            moveCtr.Update();
            if (m_lockInput == false)
                cmdMgr.Update(InputHandler.GetInputKeycode(this.id, this.facing));
            animCtr.Update();
            stateMgr.Update();
        }

        public void LockInput()
        {
            this.m_lockInput = true;
        }

        public void UnlockInput()
        {
            this.m_lockInput = false;
        }

        public void Reset()
        {
            this.hp = this.MaxHP;
            this.stateMgr.ChangeState(0);
        }

        public Collider[] GetColliders()
        {
            return new Collider[] { this.GetComponent<DecisionBoxManager>().GetCollider() };
        }
    }
}
