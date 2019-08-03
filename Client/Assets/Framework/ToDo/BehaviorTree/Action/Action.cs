using System.Collections;
using System.Collections.Generic;

namespace bluebean.UGFramework.BehaviorTree
{
    /// <summary>
    /// 执行一个action的行为树节点
    /// </summary>
    public class Action : Behavior
    {
        protected System.Action m_action;

        public Action(string nodeName, System.Action act) : base(nodeName)
        {
            m_action = act;
        }

        public override void OnInitialize() { }

        public override Status Update(Number deltaTime)
        {
            m_action();
            return Status.Success;
        }

        public override void OnTerminate() { }

    }

}
