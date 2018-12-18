﻿using System.Collections;
using System.Collections.Generic;

namespace Mugen3D.Core
{

    public class AnimationController
    {
        public Unit owner { get; private set; }
        private Dictionary<int, Action> m_actions;
        public int anim { get; private set; }
        public int animTime { get; private set; }
        public int animElem { get; private set; }
        public int animElemTime { get; private set; }

        public int animLength
        {
            get
            {
                return curAction.animLength;
            }
        }

        public int leftAnimTime
        {
            get
            {
                return animLength - animTime;
            }
        }

        public Action curAction
        {
            get
            {
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
            this.owner = owner;
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

        private void UpdateSample()
        {
            var animElemDuration = curAction.frames[animElem].duration;
            if (animElemTime >= animElemDuration) //the frame is ending
            {
                if (animElem < curAction.frames.Count - 1)
                {
                    animElem++;
                    animElemTime = 0;
                }
                else if (animElem == curAction.frames.Count - 1 && curAction.loopStartIndex != -1) //the frame is the last frame of action
                {
                    animElem = curAction.loopStartIndex;
                    animElemTime = 0;
                }
            }
            else
            {
                animTime++;
                animElemTime++;
            }
        }

        public void ChangeAnim(int anim, int animElem = 0)
        {
            if (!m_actions.ContainsKey(anim))
            {
                Log.Error("anims don't contain id: " + anim);
                return;
            }
            this.anim = anim;
            this.animElem = animElem;
            this.animTime = -1;
            this.animElemTime = -1;
        }

    }

}
