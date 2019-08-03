using System.Collections;
using System.Collections.Generic;
namespace bluebean.UGFramework.BehaviorTree
{
    /// <summary>
    /// 装饰子节点
    /// </summary>
    public abstract class Decorator : Behavior
    {

        public Behavior child;

        public Decorator(string nodeName, Behavior child) : base(nodeName)
        {
            this.child = child;
        }

    }
}
