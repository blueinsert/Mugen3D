using bluebean.UGFramework.ConfigData;

namespace bluebean.Mugen3D.Core
{
    /// <summary>
    /// 标识是否是玩家的组件,其他组建不适合存放的数据也可能存放在这里
    /// </summary>
    public class PlayerComponent : ComponentBase
    {
        public int Facing { get { return m_facing; } }
        public int Index { get { return m_index; } }
        public ConfigDataCharacter Config {get{ return m_config; } }
        private int m_facing;
        private int m_index;
        private ConfigDataCharacter m_config;

        public void Init(int index, ConfigDataCharacter config)
        {
            m_index = index;
            m_config = config;
        }
    }
}
