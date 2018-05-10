﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D
{
    public class InputHandler
    {

        public static uint GetInputKeycode(int playerSlot, int facing)
        {
            Dictionary<KeyNames, KeyCode> keycodeMap = InputConfig.Instance.mapCfg[playerSlot];          
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
