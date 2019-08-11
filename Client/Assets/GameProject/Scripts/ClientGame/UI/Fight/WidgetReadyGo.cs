using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace bluebean.Mugen3D.ClientGame
{
    public class WidgetReadyGo : UIView
    {
        public Animator animator;
        private void Awake()
        {
            animator = this.GetComponent<Animator>();
        }

        public void Play()
        {
            UIUtils.PlayAnimation(this.animator, "WidgetReadyGo_Anim");
        }
    }
}
