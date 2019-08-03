
namespace bluebean.UGFramework.BehaviorTree
{
    /// <summary>
    /// 行为树节点状态
    /// </summary>
    public enum Status
    {
        Success,
        Failure,
        Running,
    }

    /// <summary>
    /// 行为树节点基类
    /// </summary>
    public class Behavior
    {
        /// <summary>
        /// 所属的行为树
        /// </summary>
        protected BehaviorTree m_tree;
        /// <summary>
        /// 当前状态
        /// </summary>
        protected Status m_status;
        protected string m_nodeName;

        public string nodeName { get { return m_nodeName; } }

        public Behavior(string nodeName)
        {
            this.m_nodeName = nodeName;
        }

        public void SetOwnerTree(BehaviorTree tree)
        {
            this.m_tree = tree;
        }

        public virtual void OnInitialize() { }

        public virtual Status Update(Number deltaTime)
        {
            return Status.Success;
        }

        public virtual void OnTerminate() { }

        public Status Tick(Number deltaTime)
        {
            if (m_status != Status.Running)
                OnInitialize();
            m_status = Update(deltaTime);
            if (m_status != Status.Running)
                OnTerminate();
            return m_status;
        }

    }
}
