using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Mugen3D
{
    public class PlayerStateDef
    {
        private Player owner;
        public int stateId;

        public PhysicsType physicsType;
        public bool physicsIsSet = false;
        public MoveType moveType;
        public bool moveTypeIsSet = false;
        public string anim;
        public bool animIsSet = false;
        //optional param
        public Vector3 vel;
        public bool velIsSet = false;
        public bool ctrl;
        public bool ctrlIsSet = false;
        //public bool isEntered = false;

        public MyList<StateEvent> events;

        public PlayerStateDef()
        {
            events = new MyList<StateEvent>();
        }

        public void SetOwner(Player p)
        {
            owner = p;
        }

        public void OnEnter() {
            if (physicsIsSet)
            {

            }
            if (moveTypeIsSet)
            {

            }
            if (animIsSet)
            {
                Dictionary<string, string> param = new Dictionary<string, string> { {"value",anim}};
                Controllers.Instance.ChangeAnim(owner.id, param);
            }
            if (velIsSet)
            {
                Dictionary<string, string> param = new Dictionary<string, string> { { "x", vel.z.ToString() },{"y",vel.y.ToString()} };
                Controllers.Instance.VelSet(owner.id, param);
            }
            if (ctrlIsSet)
            {
                Dictionary<string, string> param = new Dictionary<string, string> { { "value", ctrl.ToString() } };
                Controllers.Instance.CtrlSet(owner.id, param);
            }
 

        }

        public void OnUpdate()
        {
            for (int i = 0; i < events.Count; i++)
            {
                StateEvent e = events[i];
                bool checkRequired = true;
                bool checkOptional = true;
                //check requiredTriggerList
                if (e.requiredTriggerList != null && e.requiredTriggerList.Count != 0)
                {
                    checkRequired = true;
                }else{
                    checkRequired = false;
                }
                if(e.optionalTriggerDic!=null && e.optionalTriggerDic.Count!=0){
                    checkOptional = true;
                }else{
                    checkOptional = false;
                }
                bool passed = (checkRequired ? CheckTriggerList(e.requiredTriggerList) : true) 
                    && (checkOptional?CheckOptionalTriggerLists(e.optionalTriggerDic):true);
                if (!passed)
                    continue;
                Controllers.Instance.ExeController(owner.id, e.type, null);            
            }
        }

        bool CheckOptionalTriggerLists(MyDictionary<int, MyList<Expression>> exDic)
        {
            foreach (var kv in exDic)
            {
                if (CheckTriggerList(kv.Value))
                {
                    return true;
                }
            }
            return false;
        }

        bool CheckTriggerList(List<Expression> expressions) {
            bool passed = true;
            foreach (var e in expressions) {
                VirtualMachine vm = new VirtualMachine();
                double result = vm.Execute(e);
                if (result == 0)
                {
                    passed = false;
                    break;
                }

            }
            return passed;
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("id:").Append(stateId).Append(",");
            sb.Append("physics:").Append(physicsType.ToString()).Append(",");
            sb.Append("moveType:").Append(moveType.ToString()).Append(",");
            sb.Append("anim:").Append(anim).Append(",");
            //sb.Append("vel:").Append(vel.ToString()).Append(",");
            sb.Append("ctrl:").Append(ctrl).Append(",");
            sb.Append("events:").Append(events.ToString()).Append(",");
            sb.Append("}");
            return sb.ToString();
        }
    }
}