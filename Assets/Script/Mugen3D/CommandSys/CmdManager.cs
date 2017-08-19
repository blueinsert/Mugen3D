using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Mugen3D
{
    public class CmdManager
    {
        List<Command> mCommands = new List<Command>();
        List<CommandState> mCommandState = new List<CommandState>();

        public void LoadCmdFile(string cmdFile)
        {
            List<Token> tokens;
            Tokenizer tokenizer = new Tokenizer();
            tokens = tokenizer.GetTokens(cmdFile);

            int defaultCommandTime = 15;
            int defaultBufferTime = 1;

            int pos = 0;
            int tokenSize = tokens.Count;
            while (pos < tokenSize)
            {
                Token t = tokens[pos++];
                if (t.value == "command.time")
                {
                    t = tokens[pos++];
                    Utility.Assert(t.value == "=", "should be = after command.time");
                    t = tokens[pos++];
                    Utility.Assert(int.TryParse(t.value, out defaultCommandTime), "command.time should be int");

                }
                else if (t.value == "command.buffer.time")
                {
                    t = tokens[pos++];
                    Utility.Assert(t.value == "=", "should be = after command.buffer.time");
                    t = tokens[pos++];
                    Utility.Assert(int.TryParse(t.value, out defaultBufferTime), "command.buffer.time should be int");
                }
                else if (t.value == "[")
                {
                    t = tokens[pos++];
                    Utility.Assert(t.value == "Command", "should be Command after [");
                    t = tokens[pos++];
                    Utility.Assert(t.value == "]", "should be ] after Command");
                    Command c = new Command();
                    c.mCommand = new List<CommandElement>();
                    c.mCommandTime = defaultCommandTime;
                    c.mBufferTime = defaultBufferTime;
                    while (pos < tokenSize)
                    {
                        t = tokens[pos++];
                        if (t.value == "[")
                        {
                            pos--;
                            break;
                        }
                        else if (t.value == "name")
                        {
                            Utility.Assert((t = tokens[pos++]).value == "=", "should be = after name");
                            c.mCommandName = (t = tokens[pos++]).value;

                        }
                        else if (t.value == "time")
                        {
                            Utility.Assert((t = tokens[pos++]).value == "=", "should be = after time");
                            int commandTime = defaultCommandTime;
                            Utility.Assert(int.TryParse((t = tokens[pos++]).value, out commandTime), "time should be int");
                            c.mCommandTime = commandTime;
                        }
                        else if (t.value == "buffer.time")
                        {
                            Utility.Assert((t = tokens[pos++]).value == "=", "should be =  after buffer.time");
                            Utility.Assert(int.TryParse((t = tokens[pos++]).value, out c.mBufferTime), "buffer.time should be int");
                        }
                        else if (t.value == "command")
                        {
                            Utility.Assert((t = tokens[pos++]).value == "=", "should be = after command");
                            CommandElement currentCommandElement = new CommandElement();
                            while (t.value != "\n")
                            {
                                t = tokens[pos++];
                                if (t.value == ",")
                                {
                                    c.mCommand.Add(currentCommandElement);
                                    currentCommandElement = new CommandElement();
                                }
                                else if (t.value == "U")
                                {
                                    currentCommandElement.keyCode += Utility.GetKeycode(KeyNames.KEY_UP);
                                }
                                else if (t.value == "D")
                                {
                                    currentCommandElement.keyCode += Utility.GetKeycode(KeyNames.KEY_DOWN);
                                }
                                else if (t.value == "B")
                                {
                                    currentCommandElement.keyCode += Utility.GetKeycode(KeyNames.KEY_LEFT);
                                }
                                else if (t.value == "F")
                                {
                                    currentCommandElement.keyCode += Utility.GetKeycode(KeyNames.KEY_RIGHT);
                                }
                                else if (t.value == "DB")
                                {
                                    currentCommandElement.keyCode += Utility.GetKeycode(KeyNames.KEY_DOWN) + Utility.GetKeycode(KeyNames.KEY_LEFT);
                                }
                                else if (t.value == "DF")
                                {
                                    currentCommandElement.keyCode += Utility.GetKeycode(KeyNames.KEY_DOWN) + Utility.GetKeycode(KeyNames.KEY_RIGHT);
                                }
                                else if (t.value == "UB")
                                {
                                    currentCommandElement.keyCode += Utility.GetKeycode(KeyNames.KEY_UP) + Utility.GetKeycode(KeyNames.KEY_LEFT);
                                }
                                else if (t.value == "UF")
                                {
                                    currentCommandElement.keyCode += Utility.GetKeycode(KeyNames.KEY_UP) + Utility.GetKeycode(KeyNames.KEY_RIGHT);
                                }
                                else if (t.value == "a")
                                {
                                    currentCommandElement.keyCode += Utility.GetKeycode(KeyNames.KEY_BUTTON_A);
                                }
                                else if (t.value == "b")
                                {
                                    currentCommandElement.keyCode += Utility.GetKeycode(KeyNames.KEY_BUTTON_B);
                                }
                                else if (t.value == "c")
                                {
                                    currentCommandElement.keyCode += Utility.GetKeycode(KeyNames.KEY_BUTTON_C);
                                }
                                else if (t.value == "x")
                                {
                                    currentCommandElement.keyCode += Utility.GetKeycode(KeyNames.KEY_BUTTON_X);
                                }
                                else if (t.value == "y")
                                {
                                    currentCommandElement.keyCode += Utility.GetKeycode(KeyNames.KEY_BUTTON_Y);
                                }
                                else if (t.value == "z")
                                {
                                    currentCommandElement.keyCode += Utility.GetKeycode(KeyNames.KEY_BUTTON_Z);
                                }
                                else if (t.value == "/")
                                {
                                    currentCommandElement.keyModifier += 1 << (int)KeyMode.KeyMode_Must_Be_Held;
                                }
                            }//while
                            c.mCommand.Add(currentCommandElement);
                        }
                    }
                    mCommands.Add(c);
                }
                else
                {
                    continue;
                }

            }
            InitCommandStates();
        }

        void InitCommandStates()
        {
            for (int i = 0; i < mCommands.Count; i++)
            {
                mCommandState.Add(new CommandState(mCommands[i]));
            }
        }

        public void Update(uint keycode)
        {
            for (int i = 0; i < mCommandState.Count; i++)
            {
                mCommandState[i].Update(keycode);
            }
        }

        public string GetActiveCommandName()
        {
            string commandName = "none";
            for (int i = 0; i < mCommandState.Count; i++)
            {
                if (mCommandState[i].IsCommandComplete)
                {
                    commandName = mCommandState[i].name;
                    break;
                }
            }
            return commandName;
        }
    }
}
