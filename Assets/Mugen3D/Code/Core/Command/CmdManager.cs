using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Mugen3D
{
    public class CmdManager
    {
        private Unit m_owner;
        private Dictionary<int, List<CommandState>> m_commandState = new Dictionary<int, List<CommandState>>();

        public void SetOwner(Unit owner)
        {
            m_owner = owner;
        }

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
                if (!m_commandState.ContainsKey(nameHash))
                {
                    m_commandState[nameHash] = new List<CommandState>();
                }
                m_commandState[nameHash].Add(cmdState);
            }
        }

        public void Update(uint keycode)
        {
            Log.Info("KEYCODE:" + keycode);
            foreach (var l in m_commandState)
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
            if (!m_commandState.ContainsKey(commandNameHashCode))
            {
                result = 0;
                Log.Warn("cmd def don't contain:" + commandNameHashCode);
            }
            else
            {
                foreach (var s in m_commandState[commandNameHashCode])
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
            foreach (var k in m_commandState.Keys)
            {
                if (CommandIsActive(k) == 1)
                {
                    sb.Append(m_commandState[k][0].name).Append(",");
                }
            }
            return sb.ToString();
        }
    }
}
