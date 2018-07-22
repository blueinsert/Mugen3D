using System.Collections;
using System.Collections.Generic;

namespace Mugen3D.Core
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
        

        public InputConfig() { }
        public static InputConfig m_instance;
        public static InputConfig Instance {
            get
            {
                if (m_instance == null)
                {
                    m_instance = ConfigReader.Read<InputConfig>(ResourceLoader.LoadText("Config/Input/Input"));
                }
                return m_instance;
            }
        }

       
    }
}
