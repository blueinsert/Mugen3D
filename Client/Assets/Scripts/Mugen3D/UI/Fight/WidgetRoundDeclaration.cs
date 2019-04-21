using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mugen3D
{
    public class WidgetRoundDeclaration : UIView
    {
        public Animator animator;
        private void Awake()
        {
            animator = this.GetComponent<Animator>();
        }

        public void Play(int roundNo, System.Action onFinish = null)
        {
            this.transform.Find("Pos/TextNo1").GetComponent<Text>().text = roundNo.ToString();
            this.transform.Find("Pos/TextNo2").GetComponent<Text>().text = roundNo.ToString();
            UIUtils.PlayAnimation(this.animator, "WidgetRoundDeclaration_Anim", onFinish);
        }
    }
}