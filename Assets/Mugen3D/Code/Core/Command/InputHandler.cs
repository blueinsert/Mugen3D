using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D
{
    public class InputHandler
    {

        public static uint GetInputKeycode(PlayerId id)
        {
            Dictionary<KeyNames, KeyCode> keycodeMap = new Dictionary<KeyNames, KeyCode>();
            switch (id)
            {
                case PlayerId.P1:
                    keycodeMap = KeycodeMapConfig.P1; break;
                default:
                    keycodeMap = KeycodeMapConfig.P2;break;
            }
            uint keycode = 0;
            //string keyInfo = "";
            foreach (var pair in keycodeMap)
            {
                if (Input.GetKey(pair.Value))
                {
                    if (pair.Key == KeyNames.KEY_LEFT && World.Instance.GetPlayer(id).facing < 0)
                    {
                        keycode = keycode | Utility.GetKeycode(KeyNames.KEY_RIGHT);
                    }
                    else if (pair.Key == KeyNames.KEY_RIGHT && World.Instance.GetPlayer(id).facing < 0)
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
