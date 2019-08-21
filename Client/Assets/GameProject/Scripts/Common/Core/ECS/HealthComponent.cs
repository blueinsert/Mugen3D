using System.Collections;
using System.Collections.Generic;

namespace bluebean.Mugen3D.Core
{
    public class HealthComponent : ComponentBase
    {
        public int HP { get { return m_hp; } }
        private int m_hp;

        public void SetHP(int hp)
        {
            m_hp = hp;
        }

        public void AddHP(int deltaHP)
        {
            m_hp += deltaHP;
        }
    }
}