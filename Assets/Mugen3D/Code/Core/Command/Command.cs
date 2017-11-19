using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{

    public enum KeyNames
    {
        KEY_UP = 0,
        KEY_DOWN,
        KEY_LEFT,
        KEY_RIGHT,
        KEY_BUTTON_A,
        KEY_BUTTON_B,
        KEY_BUTTON_C,
        KEY_BUTTON_X,
        KEY_BUTTON_Y,
        KEY_BUTTON_Z,
        KEY_BUTTON_START,
        KEY_BUTTON_PAUSE,
        KEY_COUNT
    };

    public enum KeyMode
    {
        KeyMode_Must_Be_Held = 0,
        KeyMode_Detect_As_4Way,
        KeyMode_Ban_Other_Input,
        KeyMode_On_Release,
    }

    public struct CommandElement
    {
        public uint keyCode;
        public uint keyModifier;

        public CommandElement(uint code)
        {
            keyCode = code;
            keyModifier = 0;
        }
    }

    public struct Command
    {
        public List<CommandElement> mCommand;
        public int mCommandTime, mBufferTime;
        public string mCommandName;
    }
}
