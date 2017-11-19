using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D
{
    public class CommandState
    {
        public string name;

        private  Command command; 
        private int commandElementNum;
        private int lastStateIndex = 0;
        private int currentStateIndex = 0;
        private int bufferTime = 1;
        private int commandTime = 15;

        private int bufferTimer = 0;
        private int commandBeginTime = 0;

        public bool IsCommandComplete { get { return currentStateIndex == commandElementNum; } }

        void ChangeCommandState(uint keycode)
        {
            lastStateIndex = currentStateIndex;
            if (currentStateIndex == 0)
            {
                if ((keycode & command.mCommand[currentStateIndex].keyCode) == command.mCommand[currentStateIndex].keyCode)
                {
                    currentStateIndex++;
                    if (currentStateIndex == 1)
                    {
                        commandBeginTime = Time.frameCount;
                    }
                }
            }
            else
            {
                CommandElement lastCommand = command.mCommand[currentStateIndex - 1];
                if ((lastCommand.keyModifier & (1 << (int)KeyMode.KeyMode_Must_Be_Held)) != 0)
                {
                    if (IsCommandComplete)
                    {
                        if ((keycode & lastCommand.keyCode) == lastCommand.keyCode)
                        {
                            currentStateIndex = currentStateIndex;
                            commandBeginTime = Time.frameCount;
                            bufferTimer = bufferTime;
                        }
                    }
                    else
                    {
                        if (((keycode & lastCommand.keyCode) == lastCommand.keyCode) && ((keycode & command.mCommand[currentStateIndex].keyCode) != command.mCommand[currentStateIndex].keyCode))
                        {
                            currentStateIndex = currentStateIndex;
                            commandBeginTime = Time.frameCount;
                        }
                        else if (((keycode & lastCommand.keyCode) == lastCommand.keyCode) && ((keycode & command.mCommand[currentStateIndex].keyCode) == command.mCommand[currentStateIndex].keyCode))
                        {
                            currentStateIndex++;
                        }
                        else
                        {
                            //failed,
                            currentStateIndex = 0;
                        }
                    }
                    
                }
                else
                {
                    if ((keycode & command.mCommand[currentStateIndex].keyCode) == command.mCommand[currentStateIndex].keyCode)
                    {
                        currentStateIndex++;
                    }
                }

            }
            //init bufferTimer
            if (lastStateIndex != commandElementNum && currentStateIndex == commandElementNum)
            {
                bufferTimer = bufferTime;
            }
        }

        public CommandState(Command command)
        {
            this.command = command;
            commandElementNum = command.mCommand.Count;
            bufferTime = command.mBufferTime;
            commandTime = command.mCommandTime;
            name = command.mCommandName;
        }

        public void Update(uint keycode)
        {
            if (IsCommandComplete)
            {
                bufferTimer--;
                if (bufferTimer <= 0)
                {
                    currentStateIndex = 0;
                }
            }
            ChangeCommandState(keycode);
            //check is over commandTime
            if (currentStateIndex != 0)
            {
                if (Time.frameCount - commandBeginTime > commandTime)
                {
                    currentStateIndex = 0;
                }
            }
        }

    }//class
}//namespace
