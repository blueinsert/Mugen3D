using System.Collections;
using System.Collections.Generic;
using Debug = bluebean.UGFramework.Log.Debug;

namespace bluebean.UGFramework
{
    public class TaskManager : ITickable
    {
        private TaskManager() { }

        private static TaskManager m_instance;
        public static TaskManager Instance
        {
            get { return m_instance; }
        }

        public static TaskManager CreateInstance()
        {
            m_instance = new TaskManager();
            return m_instance;
        }

        public bool Register(Task task)
        {
            if(m_taskList.Contains(task) || m_toAddTaskList.Contains(task))
            {
                Debug.LogError(string.Format("TaskManager Register Failed has registered the same task {0}", task.Name));
                return false;
            }
            m_toAddTaskList.Add(task);
            return true;
        }

        public void UnRegisterTask(Task task)
        {
            if (!m_taskList.Contains(task))
            {
                Debug.LogError(string.Format("TaskManager UnRegisterTask Failed do't contain the task {0}", task.Name));
            }
            m_toRemoveTaskList.Add(task);
        }

        public void Tick()
        {
            if (m_toAddTaskList.Count != 0) {
                m_taskList.AddRange(m_toAddTaskList);
                m_toAddTaskList.Clear();
            }
            if (m_toRemoveTaskList.Count != 0)
            {
                foreach(var task in m_toRemoveTaskList)
                {
                    m_taskList.Remove(task);
                }
            }
            foreach (var task in m_taskList)
            {
                task.Tick();
            }
        }

        private readonly List<Task> m_taskList = new List<Task>();
        private readonly List<Task> m_toAddTaskList = new List<Task>();
        private readonly List<Task> m_toRemoveTaskList = new List<Task>();
    }
}
