using System.Collections;
using System.Collections.Generic;

namespace Mugen3D.Core 
{ 

public class AnimationController {
    private Unit m_owner;

    public Dictionary<int, Action> m_actions;
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

    public Action curAction {
        get {
            return m_actions[anim];
        }
    }

    public ActionFrame curActionFrame
    {
        get
        {
            return m_actions[anim].frames[animElem];
        }
    }

    private void Init()
    {
       
    }

    public bool IsAnimExist(int anim)
    {
        return m_actions.ContainsKey(anim);
    }

    public AnimationController(Action[] actions, Unit owner)
    {
        this.m_owner = owner;
        this.m_actions = new Dictionary<int, Action>();
        foreach (var action in actions)
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
        m_owner.SendEvent(new Event(){type = EventType.SampleAnim, data = null});
    }

   

    public void  ChangeAnim(int anim)
    {
        if (!m_actions.ContainsKey(anim))
        {
            Log.Error("anims don't contain key " + anim);
            return;
        }
        this.anim = anim;
        animElem = 0;
        animElemTime = 0;
        animTime = 0;
    }

}

}
