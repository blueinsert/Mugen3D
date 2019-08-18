using System.Collections;
using System.Collections.Generic;
using System.Text;
using bluebean.UGFramework.ConfigData;

namespace bluebean.Mugen3D.Core
{

    /// <summary>
    /// 指令状态机
    ///             command[0]              command[1]          command[2]              command[n]            
    ///  state 0  --------------> state 1-------------->state 2 -------------->......-------------->state n
    /// </summary>
    public class CommandState
    {
        public string m_name;

        private Command m_command;
        private int m_commandElementNum;
        private int m_stateIndex = 0;

        private int m_bufferTime = 1;
        private int m_commandTime = 15;

        private int m_bufferTimer = 0;
        private int m_commandBeginTime = 0;

        private int m_lastInput = 0;

        public bool IsCommandComplete { get { return m_stateIndex == m_commandElementNum; } }

        public CommandState(Command command)
        {
            this.m_command = command;
            m_commandElementNum = command.mCommand.Count;
            m_bufferTime = command.mBufferTime;
            m_commandTime = command.mCommandTime;
            m_name = command.mCommandName;
        }

        private void AddStateIndex()
        {
            m_stateIndex++;
            if (m_stateIndex == 1)
            {
                m_commandBeginTime = Time.frameCount;
            }
            if (m_stateIndex == m_commandElementNum)
            {
                m_bufferTimer = m_bufferTime;
            }
        }

        private void CommandFailed()
        {
            m_stateIndex = 0;
        }

        private bool IsHolding(CommandElement expectInput, int lastInput, int curInput)
        {
            int expect = expectInput.keyCode;
            if ((expectInput.keyModifier & (1 << (int)KeyModifier.KeyMode_Detect_As_4Way)) != 0)
            {
                if ((lastInput & expect) == expect && (curInput & expect) == expect)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if ((lastInput == expect) && (curInput == expect))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private bool IsPressed(CommandElement expectInput, int lastInput, int curInput)
        {
            int expect = expectInput.keyCode;
            if ((expectInput.keyModifier & (1 << (int)KeyModifier.KeyMode_Detect_As_4Way)) != 0)
            {
                if ((lastInput & expect) != expect && (curInput & expect) == expect)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if ((lastInput != expect) && (curInput == expect))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private bool isRelease(CommandElement expectInput, int lastInput, int curInput)
        {
            int expect = expectInput.keyCode;
            if ((expectInput.keyModifier & (1 << (int)KeyModifier.KeyMode_Detect_As_4Way)) != 0)
            {
                if ((lastInput & expect) == expect && (curInput & expect) != expect)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if ((lastInput == expect) && (curInput != expect))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private void UpdateBufferTime()
        {
            m_bufferTimer--;
            if (m_bufferTimer <= 0)
            {
                CommandFailed();
            }
        }

        private void UpdateCommandTime()
        {
            if (Time.frameCount - m_commandBeginTime > m_commandTime)
            {
                CommandFailed();
            }
        }

        public void Update(int keycode)
        {
            if (IsCommandComplete)
            {
                var lastExpectInput = m_command.mCommand[m_stateIndex - 1];
                if ((lastExpectInput.keyModifier & (1 << (int)KeyModifier.KeyMode_Must_Be_Held)) != 0)
                {
                    if (IsHolding(lastExpectInput, m_lastInput, keycode))
                    {
                        m_bufferTimer = m_bufferTime;
                    }
                    else
                    {
                        CommandFailed();
                    }
                    m_lastInput = keycode;
                }
                else
                {
                    m_lastInput = keycode;
                    UpdateBufferTime();
                }
            }
            else
            {
                if (m_stateIndex != 0)
                    UpdateCommandTime();
                var expectedInput = m_command.mCommand[m_stateIndex];
                if ((expectedInput.keyModifier & (1 << (int)KeyModifier.KeyMode_On_Release)) != 0)
                {
                    if (isRelease(expectedInput, m_lastInput, keycode))
                    {
                        AddStateIndex();
                    }
                }
                else
                {
                    if (IsPressed(expectedInput, m_lastInput, keycode))
                    {
                        AddStateIndex();
                    }
                }
                m_lastInput = keycode;
            }

        }

    }//class

    public class CommandComponent : ComponentBase
    {
        private Dictionary<int, List<CommandState>> m_commandState = new Dictionary<int, List<CommandState>>();

        private readonly static List<Command> m_staticCommandList = new List<Command>();

        public static void StaticInit(List<ConfigDataCommand> commandConfigs)
        {
            m_staticCommandList.Clear();
            foreach(var commandConfig in commandConfigs)
            {
                var command = CommandParse.GetCommand(commandConfig);
                m_staticCommandList.Add(command);
            }
        }

        public void Init()
        {
            InitCommandStates(m_staticCommandList);
        }

        private void InitCommandStates(List<Command> commands)
        {
            for (int i = 0; i < commands.Count; i++)
            {
                var cmdState = new CommandState(commands[i]);
                int nameHash = cmdState.m_name.GetHashCode();
                if (!m_commandState.ContainsKey(nameHash))
                {
                    m_commandState[nameHash] = new List<CommandState>();
                }
                m_commandState[nameHash].Add(cmdState);
            }
        }

        public void Update(int keycode)
        {
            //Log.Info("KEYCODE:" + keycode);
            foreach (var l in m_commandState)
            {
                foreach (var s in l.Value)
                {
                    s.Update(keycode);
                }
            }
        }

        public bool CommandIsActive(string commandName)
        {
            return CommandIsActive(commandName.GetHashCode());
        }

        private bool CommandIsActive(int commandNameHashCode)
        {
            bool result = false;
            if (!m_commandState.ContainsKey(commandNameHashCode))
            {
                result = false;
                Debug.Log("cmd def don't contain:" + commandNameHashCode);
            }
            else
            {
                foreach (var s in m_commandState[commandNameHashCode])
                {
                    if (s.IsCommandComplete)
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }

        public string GetActiveCommandName()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var k in m_commandState.Keys)
            {
                if (CommandIsActive(k))
                {
                    sb.Append(m_commandState[k][0].m_name).Append(",");
                }
            }
            return sb.ToString();
        }
    }
}
