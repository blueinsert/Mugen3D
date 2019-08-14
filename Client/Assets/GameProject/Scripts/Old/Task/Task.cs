using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : ITask {
    public Action<Task> doTask;

    public Task(Action<Task> doTask)
    {
        this.doTask = doTask;    
    }

    public override void Exect()
    {
        doTask(this);
    }

    public override void Finish()
    {
        base.Finish();
    }
}
