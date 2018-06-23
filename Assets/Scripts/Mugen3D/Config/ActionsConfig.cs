using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D
{
    public class ActionsConfig
    {
        public List<Mugen3D.Action> actions { get; set; }

        public ActionsConfig()
        {
            this.actions = new List<Mugen3D.Action>();
        }
    }
}
