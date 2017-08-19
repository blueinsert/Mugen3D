using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D{
public class GUIDebug : MonoBehaviour
{
    public AnimationController anim;

    void OnGui() {
        if (anim != null)
        {
            DebugOutputAnimation();
        }
    }

    void DebugOutputAnimation() { 

    }
}
}
