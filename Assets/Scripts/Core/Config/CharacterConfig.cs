using System.Collections;
using System.Collections.Generic;

namespace Mugen3D.Core
{
    public class ActionsConfig
    {
        public List<Core.Action> actions { get; set; }
    }

    public class CharacterConfig
    {
        public Number scaleX { get; set; }
        public Number scaleY { get; set; }
        public Number scaleZ { get; set; }
        public string modelFile { get; set; }
        public string actionConfigFile { get; set; }
        public string cmdConfigFile { get; set; }
        public string fsmConfigFile { get; set; }

        public string commandContent { get; private set; }
        public Action[] actions { get; private set; }

        public void SetActions(Action[] actions)
        {
            this.actions = actions;
        }

        public void SetCommand(string content)
        {
            this.commandContent = content;
        }
       
    }
}
