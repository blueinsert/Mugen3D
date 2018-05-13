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

    private Animation m_anim;

    protected Dictionary<int, Mugen3D.Action> m_actions;
    public int anim;
    public int animTime;
    public int animElem;
    public int animElemTime;

    public int animLength
    {
        get
        {
            return curAction.animLength;
        }
    }

    public Mugen3D.Action curAction {
        get {
            return m_actions[anim];
        }
    }

    private void Init()
    {
        foreach (AnimationState state in this.m_anim)
        {
            state.enabled = false;
        }
    }

    public AnimationController(ActionsConfig config, Animation anim, Unit owner)
    {
        this.m_owner = owner;
        this.m_anim = anim;
        this.m_actions = new Dictionary<int, Action>();
        foreach (var action in config.actions)
        {
            m_actions.Add(action.animNo, action);
        }
    }

    public void Update()
    {
        UpdateSample();
    }
  
    private void UpdateSample() {
        animTime++;
        animElemTime++;
        var animElemDuration = curAction.frames[animElem].duration;
        if (animElemTime > animElemDuration)
        {
            if (animElem >= curAction.frames.Count - 1 && curAction.loopStartIndex != -1)
            {
                animElem = curAction.loopStartIndex;
                animElemTime = 0;
            }
            else if (animElem >= curAction.frames.Count - 1 && curAction.loopStartIndex == -1)
            {
                //do nothing
            } else {
                animElem++;
                animElemTime = 0;
            }
        }        
    }

    void Sample()
    {
        m_anim[curAction.animName].enabled = true;
        m_anim[curAction.animName].normalizedTime = curAction.frames[animElem].normalizeTime;
        m_anim[curAction.animName].weight = 1;
        m_anim.Sample();
        m_anim[curAction.animName].enabled = false;
    }

    public void  ChangeAnim(int anim)
    {
        this.anim = anim;
        animElem = 0;
        animElemTime = 0;
        animTime = 0;
    }

}

}
