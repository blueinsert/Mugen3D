using System.Collections;
using System.Collections.Generic;
using System.Text;
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
        KeyMode_Must_Be_Held,
        KeyMode_Detect_As_4Way,
        KeyMode_Ban_Other_Input,
        KeyMode_On_Release,
    }

    public class CommandElement
    {
        public uint keyCode;
        public uint keyModifier;

        public string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("keycode:").Append(keyCode).Append(",");
            sb.Append("keyModifier:").Append(keyModifier);
            sb.Append("}");
            return sb.ToString();
        }
    }

    public class Command
    {
        public List<CommandElement> mCommand;
        public int mCommandTime, mBufferTime;
        public string mCommandName;

        public string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("[");
            for (int i = 0; i < mCommand.Count; i++)
            {
                sb.Append(mCommand[i].ToString()).Append(",");
            }
            sb.Append("]").Append(",");
            sb.Append("mCommandTime:").Append(mCommandTime).Append(",");
            sb.Append("mBufferTime:").Append(mBufferTime).Append(",");
            sb.Append("mCommandName:").Append(mCommandName);
            sb.Append("}");
            return sb.ToString();
        }
    }
}
