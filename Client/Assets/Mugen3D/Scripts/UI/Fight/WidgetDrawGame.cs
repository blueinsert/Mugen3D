using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    public class WidgetDrawGame : UIView
    {
        public Animator animator;
        private void Awake()
        {
            animator = this.GetComponent<Animator>();
        }

        public void Play()
        {
            UIUtils.PlayAnimation(this.animator, "WidgetDrawGame_Anim");
        }
    }
}
