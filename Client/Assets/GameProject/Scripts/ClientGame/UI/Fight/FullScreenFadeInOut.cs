using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bluebean.Mugen3D.ClientGame
{
    public class FullScreenFadeInOut : UIView
    {
        public Animator animator;

        private void Awake()
        {
            animator = this.GetComponent<Animator>();
        }

        public void FadeIn() {
            UIUtils.PlayAnimation(animator, "FullScreenFadeInOut_FadeIn", null);
        }

        public void FadeOut()
        {
            UIUtils.PlayAnimation(animator, "FullScreenFadeInOut_FadeOut", null);
        }

    }
}
