using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Mugen3D
{
    public class CmdManager
    {
        Dictionary<int, List<CommandState>> mCommandState = new Dictionary<int, List<CommandState>>(); 

        public void LoadCmdFile(TextAsset content)
        {
            Log.Info("cmd parse begin");
            CommandParse parser = new CommandParse();
            parser.Parse(content);
            var commands = parser.GetCommands();
            InitCommandStates(commands);
            Utility.PrintCommandList(commands);
            Log.Info("cmd parse end");
        }

        void InitCommandStates(List<Command> commands)
        {
            for (int i = 0; i < commands.Count; i++)
            {
                var cmdState = new CommandState(commands[i]);
                int nameHash = cmdState.name.GetHashCode();
                if (!mCommandState.ContainsKey(nameHash))
                {
                    mCommandState[nameHash] = new List<CommandState>();
                }
                mCommandState[nameHash].Add(cmdState);
            }
        }

        public void Update(uint keycode)
        {
            Log.Info("KEYCODE:" + keycode);
            foreach (var l in mCommandState)
            {
                foreach (var s in l.Value)
                {
                    s.Update(keycode);
                }
            }
        }

        public int CommandIsActive(int commandNameHashCode)
        {
            int result = 0;
            if (!mCommandState.ContainsKey(commandNameHashCode))
            {
                result = 0;
                Log.Warn("cmd def don't contain:" + commandNameHashCode);
            }
            else
            {
                foreach (var s in mCommandState[commandNameHashCode])
                {
                    if (s.IsCommandComplete)
                    {
                        result = 1;
                        break;
                    }
                }
            }
            return result;
        }

        public string GetActiveCommandName( )
        {  
            StringBuilder sb = new StringBuilder();
            foreach (var k in mCommandState.Keys)
            {
                if (CommandIsActive(k) == 1)
                {
                    sb.Append(mCommandState[k][0].name).Append(",");
                }
            }
            return sb.ToString();
        }
    }
}
