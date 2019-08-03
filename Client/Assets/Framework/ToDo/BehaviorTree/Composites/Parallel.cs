using System.Collections;
using System.Collections.Generic;
namespace bluebean.UGFramework.BehaviorTree
{
    /// <summary>
    /// 并行节点，并行(有先后顺序)执行所有子节点，返回最后一个节点的执行状态
    /// </summary>
    public class Parallel : Composite
    {

        public Parallel(string nodeName) : base(nodeName)
        {

        }

        public override Status Update(Number deltaTime)
        {
            Status s = Status.Success;
            foreach (var b in children)
            {
                s = b.Tick(deltaTime);
            }
            return s;
        }

    }
}
