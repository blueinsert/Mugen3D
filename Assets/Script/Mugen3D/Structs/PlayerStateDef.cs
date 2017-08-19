using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Mugen3D
{
    public class PlayerStateDef
    {
        public int stateId;
        public PhysicsType physicsType;
        public MoveType moveType;
        public string anim;
        //optional param
        public DVector3 vel;
        public bool ctrl;

        public MyList<StateEvent> events;

        public PlayerStateDef()
        {
            events = new MyList<StateEvent>();
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