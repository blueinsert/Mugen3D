using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D
{
    public class PlayerMoveCtrl : MoveCtrl
    {
        private Player m_collidePlayer;
        private bool m_intersectTest = false;
 
        public PlayerMoveCtrl(Unit u):base(u)
        {

        }

        
        private void IntersectingTest(Player collidePlayer)
        {
            ABB myBB = this.m_owner.decisionBoxes.collideBox.abb;
            ABB otherBB = collidePlayer.decisionBoxes.collideBox.abb;
            var myCenter = myBB.GetCenter();
            var otherCenter = otherBB.GetCenter();
            var avgCenter = (myCenter + otherCenter) / 2;
            m_owner.moveCtr.AddPos(new Vector3((myBB.size.x/2 + SAFE_DISTANCE - Mathf.Abs(avgCenter.x - myCenter.x)) * (avgCenter.x > myCenter.x ? -1 : 1), 0, 0));
            collidePlayer.moveCtr.AddPos(new Vector3((otherBB.size.x / 2 + SAFE_DISTANCE - Mathf.Abs(avgCenter.x - otherCenter.x)) * (avgCenter.x > otherCenter.x ? -1 : 1), 0, 0)); 
        }


        protected override void AfterAddPos() {
            if (m_intersectTest)
            {
                IntersectingTest(m_collidePlayer);
                m_intersectTest = false;
            }
        }

        public override void Update()
        {
            base.Update();
        }

        protected override void OnHitCollider(RaycastHit hitResult)
        {
            base.OnHitCollider(hitResult);
            Debug.Log("hit tar, tag:" + hitResult.collider.tag + " normal:" + hitResult.normal + " dis:" + hitResult.distance);
            if (hitResult.collider.owner != null && hitResult.collider.owner is Player)
            {   
                var collidePlayer = hitResult.collider.owner as Player;
                var normal = hitResult.normal;
                m_collidePlayer = collidePlayer;
                if (Mathf.Abs(normal.x) == 1)
                {
                    Vector3 deltaPos = new Vector3(m_deltaPos.x/2, 0, 0);
                    var realDeltaPos = collidePlayer.moveCtr.AddPos(deltaPos);
                    m_deltaPos.x = realDeltaPos.x;
                }
                else if (Mathf.Abs(normal.y) == 1)
                {
                    m_deltaPos.y = -normal.y * hitResult.distance;
                    m_intersectTest = true;
                }  
            }
            else if (hitResult.collider.owner == null)
            {
                m_deltaPos = m_velocity.normalized*hitResult.distance;
                if(hitResult.normal.y > 0){
                    justOnGround = true;
                }
                if (Mathf.Abs(hitResult.normal.x) > 0.99)
                {
                    m_velocity.x = 0;
                }
               
            }
        }
       
    }
}
