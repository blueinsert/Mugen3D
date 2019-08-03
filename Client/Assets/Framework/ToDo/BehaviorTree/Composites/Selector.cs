using System.Collections;
using System.Collections.Generic;

namespace bluebean.UGFramework.BehaviorTree
{
    /// <summary>
    /// 选择子节点 顺序执行所有子节点，直到遇到第一个成功执行的子节点后返回，否侧返回failture
    /// </summary>
    public class Selector : Composite
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

        public Selector(string nodeName) : base(nodeName)
        {

        }

        public override Status Update(Number deltaTime)
        {
            while (true)
            {
                Status s = curBehavior.Tick(deltaTime);
                if (s != Status.Failure)
                    return s;
                m_curIndex++;
                if (m_curIndex >= children.Count)
                {
                    return Status.Failure;
                }
            }
        }
    }
}
