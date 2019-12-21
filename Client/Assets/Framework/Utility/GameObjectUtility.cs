using bluebean.UGFramework.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bluebean.UGFramework
{
    public class GameObjectUtility
    {
        public static void AttachUIController<T>(GameObject go) where T: UIViewController
        {
            MonoViewController.AttachViewControllerToGameObject(go, "./", typeof(T).FullName, true);
        }

        public static void DestroyChildren(GameObject go) {
            if (go.transform.childCount == 0)
                return;
            for (int i = 0; i < go.transform.childCount; i++) {
                GameObject.Destroy(go.transform.GetChild(i));
            }
        }
    }
}
