using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUtils {
    public static void PlayAnimation(Animator animator, string stateName, Action<string> finishCb) {
        var finishTrigger = animator.GetComponent<AnimatorFinishEventTrigger>();
        if( finishTrigger == null) {
            finishTrigger = animator.gameObject.AddComponent<AnimatorFinishEventTrigger>();
        }
        finishTrigger.OnFinishAnimation = finishCb;
        animator.Play(stateName);
    }
	
}
