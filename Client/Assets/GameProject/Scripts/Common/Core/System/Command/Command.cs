using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace bluebean.Mugen3D.Core
{

    

    public class CommandElement
    {
        public int keyCode;
        public int keyModifier;

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
