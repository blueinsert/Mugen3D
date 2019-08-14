using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ITask {
    public bool isFinish = false;
    public Action<ITask> onFinish; 

    public abstract void Exect();

    public virtual void Finish()
    {
        isFinish = true;
        if (this.onFinish != null)
        {
            this.onFinish(this);
        }
    }

}
