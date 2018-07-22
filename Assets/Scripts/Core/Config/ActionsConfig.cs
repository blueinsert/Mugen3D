using System.Collections;
using System.Collections.Generic;

namespace Mugen3D.Core
{

    public class ActionsConfig
    {
        public List<Action> actions { get; set; }

        public ActionsConfig()
        {
            this.actions = new List<Action>();
        }
    }

}
