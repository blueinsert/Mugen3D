using System.Collections;
using System.Collections.Generic;

namespace Mugen3D.Core
{
    /*
     *              command[0]              command[1]          command[2]              command[n]            
     * * state 0  --------------> state 1-------------->state 2 -------------->......-------------->state n
     */
    public class CommandState
    {
        public string name;

        private Command command;
        private int commandElementNum;
        private int stateIndex = 0;

        private int bufferTime = 1;
        private int commandTime = 15;

        private int bufferTimer = 0;
        private int commandBeginTime = 0;

        private uint mLastInput = 0;

        public bool IsCommandComplete { get { return stateIndex == commandElementNum; } }

        public CommandState(Command command)
        {
            this.command = command;
            commandElementNum = command.mCommand.Count;
            bufferTime = command.mBufferTime;
            commandTime = command.mCommandTime;
            name = command.mCommandName;
        }

        private void AddStateIndex()
        {
            stateIndex++;
            if (stateIndex == 1)
            {
                commandBeginTime = Time.frameCount;
            }
            if (stateIndex == commandElementNum)
            {
                bufferTimer = bufferTime;
            }
        }

        private void CommandFailed()
        {
            stateIndex = 0;
        }
  
        private bool IsHolding(CommandElement expectInput, uint lastInput, uint curInput)
        {
            uint expect = expectInput.keyCode;
            if ((expectInput.keyModifier & (1 << (int)KeyMode.KeyMode_Detect_As_4Way)) != 0)
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

        private bool IsPressed(CommandElement expectInput, uint lastInput, uint curInput)
        {
            uint expect = expectInput.keyCode;
            if ((expectInput.keyModifier & (1 << (int)KeyMode.KeyMode_Detect_As_4Way)) != 0)
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

        private bool isRelease(CommandElement expectInput, uint lastInput, uint curInput)
        {
            uint expect = expectInput.keyCode;
            if ((expectInput.keyModifier & (1 << (int)KeyMode.KeyMode_Detect_As_4Way)) != 0)
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
            bufferTimer--;
            if (bufferTimer <= 0)
            {
                CommandFailed();
            }
        }

        private void UpdateCommandTime()
        {
            if (Time.frameCount - commandBeginTime > commandTime)
            {
                CommandFailed();
            }
        }

        public void Update(uint keycode)
        {  
            if (IsCommandComplete)
            {
                var lastExpectInput = command.mCommand[stateIndex - 1];
                if ((lastExpectInput.keyModifier & (1 << (int)KeyMode.KeyMode_Must_Be_Held)) != 0)
                {
                    if (IsHolding(lastExpectInput, mLastInput, keycode))
                    {
                        bufferTimer = bufferTime;
                    }
                    else
                    {
                        CommandFailed();
                    }
                    mLastInput = keycode;
                }
                else
                {
                    mLastInput = keycode;
                    UpdateBufferTime();
                }
            }
            else
            {
                if(stateIndex != 0)
                    UpdateCommandTime();
                var expectedInput = command.mCommand[stateIndex];
                if ((expectedInput.keyModifier & (1 << (int)KeyMode.KeyMode_On_Release)) != 0)
                {
                    if (isRelease(expectedInput, mLastInput, keycode))
                    {
                        AddStateIndex();
                    }
                }
                else
                {
                    if (IsPressed(expectedInput, mLastInput, keycode))
                    {
                        AddStateIndex();
                    }
                }
                mLastInput = keycode;
            }
           
        }

    }//class
}//namespace
