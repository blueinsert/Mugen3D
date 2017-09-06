using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D
{
    public class PlayerView : MonoBehaviour
    {
        public Animation anim;
        AnimationController animCtr;
        // Use this for initialization
        void Init()
        {
            animCtr = new AnimationController(anim);
        }

        void OnUpdate()
        {
            animCtr.UpdateSample();
        }
    }
}
