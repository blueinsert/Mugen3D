using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUtils {

    public static void PlayAnimation(Animator animator, string stateName, Action finishCb = null) {
        var finishTrigger = animator.GetComponent<AnimatorFinishEventTrigger>();
        if( finishTrigger == null) {
            finishTrigger = animator.gameObject.AddComponent<AnimatorFinishEventTrigger>();
        }
        finishTrigger.OnFinishAnimation = (name) => {
            if(name == stateName)
            {
                if (finishCb != null)
                    finishCb();
            }
        };
        animator.Play(stateName, -1, 0);
    }
	
}
