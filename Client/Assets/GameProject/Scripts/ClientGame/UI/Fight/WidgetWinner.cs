﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bluebean.Mugen3D.ClientGame
{
    public class WidgetWinner : UIView
    {
        public Animator animator;
        private void Awake()
        {
            animator = this.GetComponent<Animator>();
        }

        public void Play()
        {
            UIUtils.PlayAnimation(this.animator, "WidgetWinner_Anim");
        }
    }
}