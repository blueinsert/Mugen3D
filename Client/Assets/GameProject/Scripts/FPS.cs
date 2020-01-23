    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS : MonoBehaviour {
    GUIStyle m_style;

    // Use this for initialization  
    void Start()
    {
        m_style = new GUIStyle();
        m_style.fontSize = 40;       //字体大小
    }

    // Update is called once per frame  
    void Update()
    {
        UpdateTick();
    }

    void OnGUI()
    {
        
        DrawFps();
    }

    private void DrawFps()
    {
        if (mLastFps > 50)
        {
            GUI.color = new Color(0, 1, 0);
        }
        else if (mLastFps > 40)
        {
            GUI.color = new Color(1, 1, 0);
        }
        else
        {
            GUI.color = new Color(1.0f, 0, 0);
        }
        
        GUI.Label(new Rect(Screen.width-128, 32, 64, 24), "fps: " + mLastFps,m_style);

    }

    private long mFrameCount = 0;
    private long mLastFrameTime = 0;
    static long mLastFps = 0;
    private void UpdateTick()
    {
        if (true)
        {
            mFrameCount++;
            long nCurTime = TickToMilliSec(System.DateTime.Now.Ticks);
            if (mLastFrameTime == 0)
            {
                mLastFrameTime = TickToMilliSec(System.DateTime.Now.Ticks);
            }

            if ((nCurTime - mLastFrameTime) >= 1000)
            {
                long fps = (long)(mFrameCount * 1.0f / ((nCurTime - mLastFrameTime) / 1000.0f));

                mLastFps = fps;

                mFrameCount = 0;

                mLastFrameTime = nCurTime;
            }
        }
    }
    public static long TickToMilliSec(long tick)
    {
        return tick / (10 * 1000);
    }  
}
