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
    private Player owner;
    private AnimPlayMode playMode = AnimPlayMode.Loop;
    private Animation anim;
    private AnimationsDef animDef;
    private List<string> mAnimNames = new List<string>();

    const int FrameRate = 60;
    public float speed = 2;
    public int AnimTime = -1;// 0 to omega
    public int AnimElem = 0;// 0 to (length-1)
    public float animLength;//seconds
    public int totalFrame;
    public string animName;
    public int animNo;


    public AnimationController(Animation anim, Player owner, TextAsset animDefFile)
    {
        this.owner = owner;
        animDef = new AnimationsDef();
        animDef.Init(animDefFile);
        this.anim = anim;
        Init(this.anim);
    }

    public void Init(Animation anim) {
        foreach (AnimationState state in anim)
        {
            state.enabled = false;
            mAnimNames.Add(state.name);
        }
        SetPlayAnim(anim.clip.name);
        this.animNo = -1;
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

    public void  PlayAnim(int animNo, AnimPlayMode mode = AnimPlayMode.Loop)
    {
       
        string animName = animDef.GetAnimName(animNo);
        if (mAnimNames.Contains(animName))
        {
            this.animNo = animNo;
            SetPlayAnim(animName, mode);
        }
        else
        {
            Log.Warn("animations do't contain:" + animName);
        }
    }

    private void SetPlayAnim(string animName, AnimPlayMode mode = AnimPlayMode.Loop){
        this.animName = animName;
        Debug.Log("play anim:" + animNo + " " + animName);
        animLength = anim[animName].length/speed;
        totalFrame = (int)(FrameRate * animLength);
        AnimElem = 0;
        AnimTime = -1;
        this.playMode = mode;
    }
}
}
