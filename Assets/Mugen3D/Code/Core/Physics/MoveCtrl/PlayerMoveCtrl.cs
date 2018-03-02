using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D
{
    public class PlayerMoveCtrl : MoveCtrl
    {
        protected Player mCollidePlayer;

        public PlayerMoveCtrl(Unit u):base(u)
        {

        }

        /*
        private void IntersectingTest()
        {
            if (mCollidePlayer == null)
                return;
            RectCollider selfCollider = m_owner.GetComponent<DecisionBoxManager>().GetCollider();
            RectCollider otherCollider = mCollidePlayer.GetComponent<DecisionBoxManager>().GetCollider();
            if (PhysicsUtils.RectRectTest(selfCollider, otherCollider))
            {
                float deltaX = (selfCollider.rect.width + otherCollider.rect.width) / 2 - Mathf.Abs(selfCollider.rect.position.x - otherCollider.rect.position.x);
                float centerX = (selfCollider.rect.position.x + otherCollider.rect.position.x) / 2;
                m_owner.moveCtr.AddPos(new Vector3(deltaX / 2 * (centerX > selfCollider.rect.position.x ? -1 : 1), 0, 0));
                mCollidePlayer.moveCtr.AddPos(new Vector3(deltaX / 2 * (centerX > otherCollider.rect.position.x ? -1 : 1), 0, 0));
            }
        }
         */

        public override void Update()
        {
            base.Update();
            //IntersectingTest();
        }

        protected override void OnHitCollider(RaycastHit hitResult)
        {
            base.OnHitCollider(hitResult);
            Debug.Log("hit tar, tag:" + hitResult.collider.tag + " normal:" + hitResult.normal);
            if (hitResult.collider.owner != null && hitResult.collider.owner is Player)
            {   
                var collidePlayer = hitResult.collider.owner as Player;
                var normal = hitResult.normal;
                var realDeltaPos = collidePlayer.moveCtr.AddPos(m_velocity.normalized * (m_deltaPos.magnitude - hitResult.distance));
                m_deltaPos = realDeltaPos;
                /*
                if (Mathf.Abs(normal.x) > 0.55)
                {
                    var realDeltaPos = collidePlayer.moveCtr.AddPos(m_velocity.normalized * (m_deltaPos.magnitude - hitResult.distance));
                    m_deltaPos = realDeltaPos;
                }
                 */
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
