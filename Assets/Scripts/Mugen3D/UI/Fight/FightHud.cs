using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{

    public class FightHud : UIView
    {
        public Camera uiCamera;

        private void Awake()
        {
            uiCamera = this.GetComponentInChildren<Camera>();
            UIManager.Instance.AddView("LifeBar", this.transform.Find("Canvas/Base/AnchorTop"));
            var world = ClientGame.Instance.world;
            var m_p1 = world.teamInfo.p1;
            var m_p2 = world.teamInfo.p2;
            (UIManager.Instance.AddView("HitCount", this.transform.Find("Canvas/Base/AnchorCenterLeft")) as WidgetHitCount).SetInfo(m_p1);
            (UIManager.Instance.AddView("HitCount", this.transform.Find("Canvas/Base/AnchorCenterRight")) as WidgetHitCount).SetInfo(m_p2);
        }
    }

}
