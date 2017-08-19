using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D
{
    public class InputHandler
    {

        public static uint GetInputKeycode()
        {
            uint keycode = 0;
            string keyInfo = "";
            foreach (var pair in KeycodeMapConfig.P1)
            {
                if (Input.GetKey(pair.Value))
                {
                    keycode = keycode | Utility.GetKeycode(pair.Key);
                    keyInfo += pair.Value.ToString() + "+";
                }
            }
            //Debug.Log("keycode:"+keycode);
            //Debug.Log(keyInfo);
            return keycode;
        }

    }
}
