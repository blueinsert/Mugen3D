using bluebean.UGFramework.ConfigData;

namespace bluebean.Mugen3D.Core
{
    /// <summary>
    /// 当前的动作类型
    /// </summary>
    public enum MoveType
    {
        Attack = 1,
        Idle,
        Defence,
        BeingHitted,
    }

    /// <summary>
    /// 标识是否是玩家的组件,其他组建不适合存放的数据也可能存放在这里
    /// </summary>
    public class BasicInfoComponent : ComponentBase
    {
        public MoveType MoveType { get { return m_moveType; } }
        public bool Ctrl { get { return m_ctrl; } }
        public int Index { get { return m_index; } }
        public ConfigDataCharacter Config {get{ return m_config; } }


        private int m_index;
        private ConfigDataCharacter m_config;
        /// <summary>
        /// 标识能否被控制
        /// </summary>
        private bool m_ctrl;
        /// <summary>
        /// 当前的动作类型
        /// </summary>
        private MoveType m_moveType = MoveType.Idle;
        

        public void Init(int index, ConfigDataCharacter config)
        {
            m_index = index;
            m_config = config;
        }

        public void SetCtrl(bool ctrl)
        {
            m_ctrl = ctrl;
        }

        public void MoveTypeSet(MoveType moveType)
        {
            m_moveType = moveType;
        }
    }
}
