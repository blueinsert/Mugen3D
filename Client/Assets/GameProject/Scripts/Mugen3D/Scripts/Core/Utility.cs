using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;

namespace Mugen3D.Core
{
    public class Utility
    {
        public static int GetKeycode(KeyNames key)
        {
            return 1 << ((int)key);
        }

        public static void Assert(bool flag, string msg)
        {
            if (!flag)
            {
               
            }
        }

        public static void Assert(bool flag)
        {
            Assert(flag, "assert failed");
        }

        public static void PrintTokens(Token[] tokens)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < tokens.Length; i++)
            {
                string value = tokens[i].value;
                if (value == "\n")
                {
                    value = "nextline";
                }
                sb.Append("{" + value + "," + tokens[i].type.ToString() + "}" + "\n");
            }
            Debug.Log("Command Tokens:" + sb.ToString());
        }

        public static void PrintCommandList(List<Command> cmds)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            for (int i = 0; i < cmds.Count; i++)
            {
                sb.Append(cmds[i].ToString()).Append(",");
            }
            sb.Append("]");
            Debug.Log("CommandList:" + sb.ToString());
        }

        public static string DicToString<T1, T2>(Dictionary<T1, T2> dic)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            foreach (var kv in dic)
            {
                T1 k = kv.Key;
                sb.Append(k.ToString());
                sb.Append(":");
                T2 v = kv.Value;
                sb.Append(v.ToString());
                sb.Append(","); 
            }
            sb.Append("}");
            return sb.ToString();
        }


    }
}
