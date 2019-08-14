using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bluebean.UGFramework
{
    public class UISceneLayer : SceneLayer
    {
        public override Camera LayerCamera
        {
            get
            {
                return GetComponentInParent<Camera>();
            }
        }

    }
}
