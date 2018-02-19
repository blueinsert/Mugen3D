using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    public enum MoveType
    {
        Attack,
        Idle,
        Defence,
        BeingHitted,
    }

    public enum PhysicsType
    {
        None = -1,
        Stand,
        Crouch,
        Air,
    }

    public class Status
    {
        public MoveType moveType = MoveType.Idle;
        public PhysicsType physicsType = PhysicsType.Stand;
        public bool pushTest = true;
        public bool ctrl = true;
    }

    public class Config
    {
        private Dictionary<int, float> cfg = new Dictionary<int, float>();

        public Config(TextAsset def)
        {
            Parse(def);
        }

        public void AddConfig(int key, float value)
        {
            cfg[key] = value;
        }

        public float GetConfig(int key)
        {
            if (cfg.ContainsKey(key))
            {
                return cfg[key];
            }
            else
            {
                Log.Error("can't get playerConfit, key:" + key);
                return 0;
            }
        }

        public float GetConfig(string key)
        {
            return GetConfig(key.GetHashCode());
        }

        private void Parse(TextAsset def)
        {
            Tokenizer tokenizer = new Tokenizer();
            List<Token> tokens = tokenizer.GetTokens(def);
            int tokenSize = tokens.Count;
            int pos = 0;
            while (pos < tokenSize)
            {
                Token t = tokens[pos++];
                if (t.value == ":")
                {
                    string key = tokens[pos - 2].value;
                    float value = float.Parse(tokens[pos++].value);
                    cfg[key.GetHashCode()] = value;
                }
            }
        }

    }//class   

    [RequireComponent(typeof(DecisionBoxes))]
    public abstract class Unit : Entity
    {
        public TextAsset configFile;
        public TextAsset animDefFile;   
        public TextAsset commandFile;
        public List<TextAsset> stateFiles;

        public Config config;
        public AnimationController animCtr;
        public CmdManager cmdMgr;
        public MoveCtrl moveCtr;
        public StateManager stateMgr;
        [HideInInspector]
        public DecisionBoxes decisionBoxes;

        public Status status = new Status();
        public Dictionary<int, int> vars = new Dictionary<int,int>();

        public HitVars hitVars;   
        [HideInInspector]
        public int facing = 1;
        [HideInInspector]
        public Unit enemy;

        private int pauseTime = 0;

        public override void Init()
        {
            decisionBoxes = this.GetComponent<DecisionBoxes>();
            //decisionBoxes.Init();
        }

        public override Collider GetCollider()
        {
            var c = new AABBCollider();
            c.aabb = decisionBoxes.GetMinAABB();
            return c;
        }

        public override void OnUpdate()
        {
            int facing = enemy.transform.position.x - this.transform.position.x > 0 ? 1 : -1;
            if (this.facing != facing)
            {
                ChangeFacing(facing);
            }
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
       
        public void SetEnemy(Unit enemy)
        {
            this.enemy = enemy;
        }

        public void SetHitVars(HitVars hitvars)
        {
            this.hitVars = hitvars;
        }

        public double CalcExpressionInRuntime(Expression ex)
        {
            VirtualMachine vm = new VirtualMachine();
            vm.SetOwner(this);
            return vm.Execute(ex);
        }

        public int GetVar(int id)
        {
            if (vars != null && vars.ContainsKey(id))
            {
                return vars[id];
            }
            return -1;
        }

        public void SetVar(int id, int value)
        {
            if (vars == null)
            {
                vars = new Dictionary<int, int>();
            }
            vars[id] = value;
        }

        public bool IsPause()
        {
            return pauseTime > 0;
        }

        public void Pause(int duration)
        {
            pauseTime = duration;
        }

        public void SetPhysicsType(PhysicsType type)
        {
            status.physicsType = type;
        }

        public void SetCtrl(bool ctrl)
        {
            status.ctrl = ctrl;
        }
    }
}
