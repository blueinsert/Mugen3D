using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace bluebean.UGFramework.BehaviorTree
{
    /// <summary>
    /// 定时器子节点
    /// </summary>
    class Timer : Decorator
    {
        private Number m_period;
        private Number m_lastExcuteTime;

        public Timer(string nodeName, Number period, Number delayTime, Behavior child) : base(nodeName, child)
        {
            m_period = period;
            m_lastExcuteTime = -period + delayTime;
        }

        public override Status Update(Number deltaTime)
        {
            Status result = Status.Running;
            if (m_tree.curUpdateTime - m_lastExcuteTime >= m_period)
            {
                m_lastExcuteTime = m_tree.curUpdateTime;
                result = child.Tick(deltaTime);
            }
            return result;
        }
    }
}
