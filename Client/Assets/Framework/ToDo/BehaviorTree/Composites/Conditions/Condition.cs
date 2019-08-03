using System.Collections;
using System.Collections.Generic;
namespace bluebean.UGFramework.BehaviorTree
{
    /// <summary>
    /// 条件子节点
    /// </summary>
    public class Condition : Behavior
    {
        protected System.Func<bool> m_condition;

        public Condition(string nodeName, System.Func<bool> condition) : base(nodeName)
        {
            m_condition = condition;
        }

        public override void OnInitialize() { }

        public override Status Update(Number deltaTime)
        {
            bool res = m_condition();
            if (res)
            {
                return Status.Success;
            }
            else
            {
                return Status.Failure;
            }
        }

        public override void OnTerminate() { }
    }
}
