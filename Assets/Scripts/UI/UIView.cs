using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIView : MonoBehaviour {
    public Animator animator;
    public AnimationClip inAnim;
    public AnimationClip outAnim;
    public System.Action onFadeInComplete;
    public System.Action onDestroy;

    protected void Show() {
        this.gameObject.SetActive(true);
        if (inAnim != null)
        {
            UIUtils.PlayAnimation(this.animator, inAnim.name, (animName) => {
                if (onFadeInComplete != null)
                {
                    onFadeInComplete();
                }
            });
        }
        else
        {
            onFadeInComplete();
        }
       
    }

    public void Close(){
        if (outAnim != null)
        {
            UIUtils.PlayAnimation(this.animator, outAnim.name, (animName) => { this.Destroy(); });
        }
        else
        {
            Destroy();
        }
    }

    private void Destroy() {
        if (onDestroy != null)
        {
            onDestroy();
        }
        GameObject.Destroy(this.gameObject);
    }
	
}
