using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    public class InputHandler
    {
        private static InputHandler m_instance;
        public static InputHandler Instance
        {
            get
            {
                if (m_instance == null) {
                    m_instance = new InputHandler();
                }
                return m_instance;
            }
        }

        private Dictionary<int, Dictionary<Core.KeyNames, KeyCode>> m_mapCfg = new Dictionary<int, Dictionary<Core.KeyNames, KeyCode>>();

        private InputHandler()
        {
            InitMapCfg();
        }

        private void InitMapCfg()
        {
            var inputs = Core.SystemConfig.Instance.inputConfig;
            for (int i = 0; i < inputs.Count; i++)
            {
                var mapping = new Dictionary<Core.KeyNames, KeyCode>();
                mapping.Add(Core.KeyNames.KEY_UP, (KeyCode)inputs[i].up);
                mapping.Add(Core.KeyNames.KEY_DOWN, (KeyCode)inputs[i].down);
                mapping.Add(Core.KeyNames.KEY_LEFT, (KeyCode)inputs[i].left);
                mapping.Add(Core.KeyNames.KEY_RIGHT, (KeyCode)inputs[i].right);
                mapping.Add(Core.KeyNames.KEY_BUTTON_A, (KeyCode)inputs[i].a);
                mapping.Add(Core.KeyNames.KEY_BUTTON_B, (KeyCode)inputs[i].b);
                mapping.Add(Core.KeyNames.KEY_BUTTON_C, (KeyCode)inputs[i].c);
                mapping.Add(Core.KeyNames.KEY_BUTTON_X, (KeyCode)inputs[i].x);
                mapping.Add(Core.KeyNames.KEY_BUTTON_Y, (KeyCode)inputs[i].y);
                mapping.Add(Core.KeyNames.KEY_BUTTON_Z, (KeyCode)inputs[i].z);
                m_mapCfg.Add(inputs[i].slot, mapping);
            }
        }

        public int GetInputKeycode(int slot)
        {
            Dictionary<Core.KeyNames, KeyCode> keycodeMap = m_mapCfg[slot];
            int keycode = 0;
            foreach (var pair in keycodeMap)
            {
                if (Input.GetKey(pair.Value))
                {
                    /*
                    if (pair.Key == Core.KeyNames.KEY_LEFT && facing < 0)
                    {
                        keycode = keycode | Core.Utility.GetKeycode(Core.KeyNames.KEY_RIGHT);
                    }
                    else if (pair.Key == Core.KeyNames.KEY_RIGHT && facing < 0)
                    {
                        keycode = keycode | Core.Utility.GetKeycode(Core.KeyNames.KEY_LEFT);
                    }
                    */
                    keycode = keycode | Core.Utility.GetKeycode(pair.Key);
                }
            }
            return keycode;
        }

    }
}
