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
        public int enter { get; set; }
        public int cancel { get; set; }
        public int a { get; set; }
        public int b { get; set; }
        public int c { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int z { get; set; }
    }

    public class InputConfig
    {
        public List<PlayerInputConfig> cfg { get; set;}
        public List<Dictionary<KeyNames, KeyCode>> mapCfg;

        public static InputConfig Instance;
        
      
        public static void Init(string configFile){
            var inputConfig = ConfigReader.Read<InputConfig>(ResourceLoader.LoadText(configFile));
            Instance = inputConfig;
            Instance.InitMapCfg();
        }

        public void InitMapCfg() {
            mapCfg = new List<Dictionary<KeyNames, KeyCode>>();
            for(int i=0; i<cfg.Count; i++){
                var mapping = new Dictionary<KeyNames,KeyCode>();
                mapping.Add(KeyNames.KEY_UP, (KeyCode)cfg[i].up);
                mapping.Add(KeyNames.KEY_DOWN, (KeyCode)cfg[i].down);
                mapping.Add(KeyNames.KEY_LEFT, (KeyCode)cfg[i].left);
                mapping.Add(KeyNames.KEY_RIGHT, (KeyCode)cfg[i].right);
                mapping.Add(KeyNames.KEY_BUTTON_A, (KeyCode)cfg[i].a);
                mapping.Add(KeyNames.KEY_BUTTON_B, (KeyCode)cfg[i].b);
                mapping.Add(KeyNames.KEY_BUTTON_C, (KeyCode)cfg[i].c);
                mapping.Add(KeyNames.KEY_BUTTON_X, (KeyCode)cfg[i].x);
                mapping.Add(KeyNames.KEY_BUTTON_Y, (KeyCode)cfg[i].y);
                mapping.Add(KeyNames.KEY_BUTTON_Z, (KeyCode)cfg[i].z);
                mapCfg.Add(mapping);
            }
        }
    }
}
