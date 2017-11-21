using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Mugen3D
{
    public class CmdManager
    {
        Dictionary<int, CommandState> mCommandState = new Dictionary<int, CommandState>(); 

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
                mCommandState[cmdState.name.GetHashCode()] = cmdState;
            }
        }

        public void Update(uint keycode)
        {
            Log.Info("KEYCODE:" + keycode);
            foreach (var s in mCommandState)
            {  
                s.Value.Update(keycode);
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
                result = mCommandState[commandNameHashCode].IsCommandComplete ? 1 : 0;
            }
            return result;
        }

        public string GetActiveCommandName( )
        {  
            StringBuilder sb = new StringBuilder();
            foreach (var kv in mCommandState)
            {
                if (kv.Value.IsCommandComplete)
                {
                    sb.Append(kv.Value.name).Append(",");
                }
            }
            return sb.ToString();
        }
    }
}
