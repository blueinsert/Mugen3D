using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluebean.Mugen3D.Core
{
    public class HitInfo
    {
        public HitType hitType;
        public int hitFlags;
    }
    /// <summary>
    /// 只是免疫哪些类型的攻击
    /// </summary>
    class NoHitByComponent:ComponentBase
    {
        protected List<HitInfo> m_infos;
        protected int m_timer;

        public bool IsActive()
        {
            return m_timer > 0;
        }

        public void SetInfo(List<HitInfo> infos, int duration)
        {
            this.m_infos = infos;
            this.m_timer = duration;
        }

        public void Update()
        {
            if (m_timer > 0)
                m_timer--;
        }

        public bool Check(HitDefData hitDef)
        {
            if (!IsActive())
                return false;
            foreach (var hitInfo in this.m_infos)
            {
                if ((hitInfo.hitFlags & hitDef.hitFlag) != 0 && hitInfo.hitType == hitDef.hitType)
                    return true;
            }
            return false;
        }
    }
}
