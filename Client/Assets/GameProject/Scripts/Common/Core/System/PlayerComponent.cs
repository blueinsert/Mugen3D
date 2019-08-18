namespace bluebean.Mugen3D.Core
{
    /// <summary>
    /// 标识是否是玩家的组件
    /// </summary>
    public class PlayerComponent : ComponentBase
    {
        public int Slot { get { return m_slot; } }
        private int m_slot;

        public void Init(int slot)
        {
            m_slot = slot;
        }
    }
}
