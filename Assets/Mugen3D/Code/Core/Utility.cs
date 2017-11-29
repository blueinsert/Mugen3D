using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mugen3D;
using System.Text;
using System;

namespace Mugen3D
{
    public class Utility
    {
        public static uint GetKeycode(KeyNames key)
        {
            return (uint)1 << ((int)key);
        }

        public static void Assert(bool flag, string msg)
        {
            if (!flag)
            {
                Debug.LogError(msg);
                Application.Quit();
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
            Log.Info("CommandList:" + sb.ToString());
        }

        public static string TokensToString(List<Token> tokens)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < tokens.Count; i++)
            {
                sb.Append(tokens[i].value);
            }
            return sb.ToString();
        }

    }
}
