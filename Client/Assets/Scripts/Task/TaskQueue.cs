using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskQueue : ITask  {

    private List<ITask> m_tasks = new List<ITask>();

    public TaskQueue AddTask(ITask task)
    {
        m_tasks.Add(task);
        return this;
    }

    private void DoNext()
    {
        if (m_tasks.Count == 0)
        {
            return;
        }
        var curTask = m_tasks[0];
        curTask.onFinish = (task) => {
            m_tasks.RemoveAt(0);
            DoNext();
        };
        curTask.Exect();
    }

    public override void Exect()
    {
        DoNext();
    }

    public override void Finish()
    {
        throw new NotImplementedException();
    }

}
