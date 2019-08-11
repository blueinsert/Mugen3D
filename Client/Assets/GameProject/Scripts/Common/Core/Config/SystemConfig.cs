using System.Collections;
using System.Collections.Generic;

namespace bluebean.Mugen3D.Core
{
    public class PlayerInputConfig
    {
        public int slot { get; set; }
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

    public class SystemConfig
    {
        public Dictionary<int, PlayerInputConfig> inputConfig { get; set; }

        public SystemConfig() { }
        private static SystemConfig m_instance;
        public static SystemConfig Instance {
            get {
                if (m_instance == null)
                    m_instance = new SystemConfig();
                return m_instance;
            }
        }

        public void Init(string content) {
            m_instance = ConfigReader.Parse<SystemConfig>(content);
        }
    }
}