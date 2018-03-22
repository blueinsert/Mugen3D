using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D { 

public enum AnimPlayMode
{
    Loop,
    Once,
}

public class AnimationController {
    private Unit m_owner;
    private AnimPlayMode m_playMode = AnimPlayMode.Loop;
    private Animation m_anim;
    private List<string> m_allAnims = new List<string>();

    const int FrameRate = 60;
    public float speed = 1;

    public float animLength; //in seconds
    public int totalFrame; // length * FrameRate
    public string animName;

    public int animTime = -1;// update count since the anim start
    public int animFrame = 0;// index in totalFrame : from 0 to (totalFrame - 1)
    


    public AnimationController(Animation anim, Unit owner)
    {
        this.m_owner = owner;
        this.m_anim = anim;
        foreach (AnimationState state in anim)
        {
            state.enabled = false;
            m_allAnims.Add(state.name);
        }
    }

    public void Update()
    {
        if(m_allAnims.Contains(animName))
            UpdateSample();
    }
  
    private void UpdateSample() {
        animTime++;
        int sampleFrameIndex = 0;
        if (m_playMode == AnimPlayMode.Loop)
        {
            sampleFrameIndex = animTime % totalFrame;
        }
        else if(m_playMode == AnimPlayMode.Once) {
            if (animTime >= totalFrame)
            {
                sampleFrameIndex = totalFrame - 1;
            }
            else
            {
                sampleFrameIndex = animTime % totalFrame;
            }
        }
        animFrame = sampleFrameIndex;
        Sample(animFrame);
    }

    void Sample(int sampleFrameIndex)
    {
        float normalizedTime = sampleFrameIndex / (float)totalFrame;
        m_anim[animName].enabled = true;
        m_anim[animName].normalizedTime = normalizedTime;
        m_anim[animName].weight = 1;
        m_anim.Sample();
        m_anim[animName].enabled = false;
    }

    public void  ChangeAnim(string animName, string playMode)
    {
        if (m_allAnims.Contains(animName))
        {
            var mode = AnimPlayMode.Loop;
            if (playMode == "Once")
                mode = AnimPlayMode.Once;
            SetAnim(animName, mode);
        }
        else
        {
            Log.Warn("animations do't contain:" + animName);
        }
    }

    private void SetAnim(string animName, AnimPlayMode mode = AnimPlayMode.Loop){
        this.animName = animName;
        this.m_playMode = mode;
        animLength = m_anim[animName].length/speed;
        totalFrame = (int)(FrameRate * animLength);
        animFrame = 0;
        animTime = -1;   
    }

}
}
