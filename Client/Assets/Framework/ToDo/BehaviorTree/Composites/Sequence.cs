using System.Collections;
using System.Collections.Generic;
namespace bluebean.UGFramework.BehaviorTree
{
    /// <summary>
    /// 序列子节点，顺序执行所有子节点，如果中间出错则提前返回
    /// </summary>
    public class Sequence : Composite
    {
        protected int m_curIndex;
        protected Behavior curBehavior
        {
            get
            {
                return m_children[m_curIndex];
            }
        }

        public override void OnInitialize()
        {
            m_curIndex = 0;
        }

        public Sequence(string nodeName) : base(nodeName)
        {

        }

        public override Status Update(Number deltaTime)
        {
            while (true)
            {
                Status s = curBehavior.Tick(deltaTime);
                if (s == Status.Failure)
                {
                    return Status.Failure;
                }
                m_curIndex++;
                if (m_curIndex >= children.Count)
                {
                    return Status.Success;
                }
            }
        }
    }
}
