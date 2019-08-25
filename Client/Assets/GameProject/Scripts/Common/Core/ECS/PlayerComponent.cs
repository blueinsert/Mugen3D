using bluebean.UGFramework.ConfigData;

namespace bluebean.Mugen3D.Core
{
    /// <summary>
    /// 标识是否是玩家的组件
    /// </summary>
    public class PlayerComponent : ComponentBase
    {
        public int Index { get { return m_index; } }
        public ConfigDataCharacter Config {get{ return m_config; } }

        private int m_index;
        private ConfigDataCharacter m_config;

        public void Init(int index, ConfigDataCharacter config)
        {
            m_index = index;
            m_config = config;
        }
    }
}
