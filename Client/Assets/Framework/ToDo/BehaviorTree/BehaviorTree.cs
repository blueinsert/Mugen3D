using System.Collections;
using System.Collections.Generic;

namespace bluebean.UGFramework.BehaviorTree
{
    /// <summary>
    /// 行为树
    /// </summary>
    public class BehaviorTree
    {
        //根节点
        private Behavior root;
        private Number m_curUpdateTime;
        public Number curUpdateTime { get { return m_curUpdateTime; } }

        public BehaviorTree(Behavior root)
        {
            this.root = root;
            SetOwnerTreeRecursive(root);
        }

        private void SetOwnerTreeRecursive(Behavior b)
        {
            b.SetOwnerTree(this);
            if (b is Composite)
            {
                var c = b as Composite;
                foreach (var child in c.children)
                {
                    SetOwnerTreeRecursive(child);
                }
            }
            else if (b is Decorator)
            {
                var d = b as Decorator;
                SetOwnerTreeRecursive(d.child);
            }
        }

        public void Tick(Number deltaTime)
        {
            m_curUpdateTime += deltaTime;
            root.Tick(deltaTime);
        }

    }
}
