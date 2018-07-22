using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mugen3D.Core;

namespace Mugen3D
{
    public class InputHandler
    {
        private static List<Dictionary<KeyNames, KeyCode>> m_mapCfg = new List<Dictionary<KeyNames, KeyCode>>();

        private static List<Dictionary<KeyNames, KeyCode>> mapCfg
        {
            get
            {
                if (m_mapCfg == null || m_mapCfg.Count == 0)
                {
                    InitMapCfg();
                }
                return m_mapCfg;
            }
        }

        private static void InitMapCfg()
        {
            m_mapCfg = new List<Dictionary<KeyNames, KeyCode>>();
            var inputs = InputConfig.Instance.inputs;
            for (int i = 0; i < inputs.Count; i++)
            {
                var mapping = new Dictionary<KeyNames, KeyCode>();
                mapping.Add(KeyNames.KEY_UP, (KeyCode)inputs[i].up);
                mapping.Add(KeyNames.KEY_DOWN, (KeyCode)inputs[i].down);
                mapping.Add(KeyNames.KEY_LEFT, (KeyCode)inputs[i].left);
                mapping.Add(KeyNames.KEY_RIGHT, (KeyCode)inputs[i].right);
                mapping.Add(KeyNames.KEY_BUTTON_A, (KeyCode)inputs[i].a);
                mapping.Add(KeyNames.KEY_BUTTON_B, (KeyCode)inputs[i].b);
                mapping.Add(KeyNames.KEY_BUTTON_C, (KeyCode)inputs[i].c);
                mapping.Add(KeyNames.KEY_BUTTON_X, (KeyCode)inputs[i].x);
                mapping.Add(KeyNames.KEY_BUTTON_Y, (KeyCode)inputs[i].y);
                mapping.Add(KeyNames.KEY_BUTTON_Z, (KeyCode)inputs[i].z);
                mapCfg.Add(mapping);
            }
        }

        public static uint GetInputKeycode(int playerSlot, int facing)
        {
            Dictionary<KeyNames, KeyCode> keycodeMap = mapCfg[playerSlot];          
            uint keycode = 0;
            foreach (var pair in keycodeMap)
            {
                if (Input.GetKey(pair.Value))
                {
                    if (pair.Key == KeyNames.KEY_LEFT && facing < 0)
                    {
                        keycode = keycode | Utility.GetKeycode(KeyNames.KEY_RIGHT);
                    }
                    else if (pair.Key == KeyNames.KEY_RIGHT && facing < 0)
                    {
                        keycode = keycode | Utility.GetKeycode(KeyNames.KEY_LEFT);
                    }
                    else
                    {
                        keycode = keycode | Utility.GetKeycode(pair.Key);
                    }   
                    //keyInfo += pair.Value.ToString() + "+";
                }
            }
            //Debug.Log("keycode:"+keycode);
            //Debug.Log(keyInfo);
            return keycode;
        }

    }
}
