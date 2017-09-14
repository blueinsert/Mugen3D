using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D { 

public enum AnimPlayMode
{
    Loop,
    stopInEnd,
}

public class AnimationController {
    private AnimPlayMode playMode = AnimPlayMode.Loop;
    private Animation anim;
    public int AnimTime = 0;
    public int AnimElem = 0;
    public float animLength;//seconds
    public int totalFrame;
    public string animName;
    public int FrameRate = 50;

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
        else if(playMode == AnimPlayMode.stopInEnd) {
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

    public void SetPlayAnim(string animName)
    {
        this.animName = animName;
        animLength = anim[animName].length;
        totalFrame = (int)(FrameRate * animLength);
        AnimElem = 0;
        AnimTime = 0;
    }
}
}
