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
    }

    public class Config
    {
        private Dictionary<int, int> cfg = new Dictionary<int, int>();

        public Config(TextAsset def)
        {
            Parse(def);
        }

        public int GetConfig(int key)
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

        public int GetConfig(string key)
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
                    int value = int.Parse(tokens[pos++].value);
                    cfg[key.GetHashCode()] = value;
                }
            }
        }

    }//class   

    public abstract class Unit : Entity
    {
        public TextAsset configFile;
        public TextAsset animDefFile;   
        public TextAsset commandFile;
        public List<TextAsset> stateFiles;

        public Config config;
        public AnimationController animCtr;
        public CmdManager cmdMgr;
        public MoveCtr moveCtr;
        public StateManager stateMgr;

        public Status status = new Status();
        public Dictionary<int, int> vars = new Dictionary<int,int>();

        public HitVars hitVars;   
        [HideInInspector]
        public int facing = 1;
        [HideInInspector]
        public Unit enemy;

        private int pauseTime = 0;
       
        public abstract void Init();

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

        public abstract void OnUpdate();

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
    }
}
