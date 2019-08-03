using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallelTask : ITask  {

    private List<ITask> m_tasks = new List<ITask>();

    public void AddTask(ITask task)
    {
        m_tasks.Add(task);
    }

    private bool CheckIsFinish()
    {
        foreach (var task in m_tasks)
        {
            if (task.isFinish == false)
            {
                return false;
            }
        }
        return true;
    }

    public override void Exect()
    {
        foreach (var task in m_tasks)
        {
            task.onFinish = (t) => {
                if (CheckIsFinish())
                {
                    Finish();
                }
            };
        }
        foreach (var task in m_tasks)
        {
            task.Exect();
        }
    }

    public override void Finish()
    {
        base.Finish();
    }
}
