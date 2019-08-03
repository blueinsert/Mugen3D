using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdGenerater {
    private IdGenerater() { }
    private static IdGenerater mInstance;
    public static IdGenerater Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new IdGenerater();
            }
            return mInstance;
        }
    }

    private int mId = 0;

    public int NextId()
    {
        return mId++;
    }
}
