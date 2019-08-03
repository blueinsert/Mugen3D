using System.Collections;
using System.Collections.Generic;
namespace bluebean.UGFramework.BehaviorTree
{
    /// <summary>
    /// 重复子节点
    /// </summary>
    public class Repeat : Decorator
    {
        protected int m_repeatNum;
        protected int m_counter = 0;

        public Repeat(string nodeName, Behavior child, int repeatNum) : base(nodeName, child)
        {
            m_repeatNum = repeatNum;
        }

        public override Status Update(Number deltaTime)
        {
            while (true)
            {
                var status = child.Tick(deltaTime);
                if (status == Status.Running)
                    break;
                if (status == Status.Failure)
                    return Status.Failure;
                m_counter++;
                if (m_counter >= m_repeatNum)
                    return Status.Success;
            }
            return Status.Running;
        }

    }
}

