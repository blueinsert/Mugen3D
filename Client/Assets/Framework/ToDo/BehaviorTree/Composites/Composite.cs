using System.Collections;
using System.Collections.Generic;
namespace bluebean.UGFramework.BehaviorTree
{
    /// <summary>
    /// 组合子节点
    /// </summary>
    public abstract class Composite : Behavior
    {
        protected List<Behavior> m_children = new List<Behavior>();

        public List<Behavior> children { get { return m_children; } }

        public Composite AddChild(Behavior b)
        {
            m_children.Add(b);
            return this;
        }

        public void RemoveChild(Behavior b)
        {
            m_children.Remove(b);
        }

        public void ClearChildren()
        {
            m_children.Clear();
        }

        public Composite(string nodeName) : base(nodeName)
        {
        }

    }
}
