using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D
{
    public class PlayerInputConfig
    {
        public int up { get; set; }
        public int down { get; set; }
        public int left { get; set; }
        public int right { get; set; }
        public int a { get; set; }
        public int b { get; set; }
        public int c { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int z { get; set; }
    }

    public class InputConfig
    {
        public List<PlayerInputConfig> inputs { get; set;}
        public List<Dictionary<KeyNames, KeyCode>> mapCfg;

        public InputConfig() { }
        public static InputConfig m_instance;
        public static InputConfig Instance {
            get
            {
                if (m_instance == null)
                {
                    m_instance = ConfigReader.Read<InputConfig>(ResourceLoader.LoadText("Config/Input/Input"));
                    m_instance.InitMapCfg();
                }
                return m_instance;
            }
        }

        public void InitMapCfg() {
            mapCfg = new List<Dictionary<KeyNames, KeyCode>>();
            for(int i=0; i < inputs.Count; i++){
                var mapping = new Dictionary<KeyNames,KeyCode>();
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
    }
}
