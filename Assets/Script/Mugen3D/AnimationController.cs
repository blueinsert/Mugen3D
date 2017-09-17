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
    private AnimPlayMode playMode = AnimPlayMode.Loop;
    private Animation anim;
    public int AnimTime = -1;// 0 to omega
    public int AnimElem = 0;// 0 to (length-1)
    public float animLength;//seconds
    public int totalFrame;
    public string animName;
    public int FrameRate = 60;

    public AnimationController(Animation anim)
    {
        this.anim = anim;
        Init(this.anim);
    }

    public void Init(Animation anim) {
        foreach (AnimationState state in anim)
        {
            state.enabled = false;
        }
        SetPlayAnim(anim.clip.name);
    }

    public void Update()
    {
        UpdateSample();
    }
  
    private void UpdateSample() {
        AnimTime++;
        int sampleFrameIndex = 0;
        if (playMode == AnimPlayMode.Loop)
        {
            sampleFrameIndex = AnimTime % totalFrame;
        }
        else if(playMode == AnimPlayMode.Once) {
            if (AnimTime >= totalFrame)
            {
                sampleFrameIndex = totalFrame - 1;
            }
            else
            {
                sampleFrameIndex = AnimTime % totalFrame;
            }
        }
        AnimElem = sampleFrameIndex;
        Sample(sampleFrameIndex);
    }

    void Sample(int sampleFrameIndex)
    {
        float normalizedTime = sampleFrameIndex / (float)totalFrame;
        anim[animName].enabled = true;
        anim[animName].normalizedTime = normalizedTime;
        anim[animName].weight = 1;
        anim.Sample();
        anim[animName].enabled = false;
    }

    public void SetPlayAnim(string animName, AnimPlayMode mode = AnimPlayMode.Loop)
    {
        this.animName = animName;
        animLength = anim[animName].length;
        totalFrame = (int)(FrameRate * animLength);
        AnimElem = 0;
        AnimTime = -1;
        this.playMode = mode;
    }
}
}
