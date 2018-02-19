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

        /*
        protected override void HandleHitUp(RaycastHit hit)
        {
            if (hit.collider.owner != null && hit.collider.owner is Player)
            {
            }
            else
            {
                m_deltaPos.y = hit.point.y - (m_minAABB.rect.position.y + m_minAABB.rect.height / 2);
                m_velocity.y = 0;
            }
        }

        protected override void HandleHitBelow(RaycastHit hit)
        {
            if (hit.collider.owner != null && hit.collider.owner is Player)
            {
              
            }
            else
            {
                m_deltaPos.y = -hit.point.y + (m_minAABB.rect.position.y - m_minAABB.rect.height / 2);
                m_velocity.y = 0;
                m_owner.status.physicsType = PhysicsType.Stand;
                this.justOnGround = true;
            }
        }

        protected override void HandleHitLeft(RaycastHit hit)
        {
            if (hit.collider.owner != null && hit.collider.owner is Player)
            {
                m_deltaPos.x = m_deltaPos.x / 2;
                Player collidePlayer = hit.collider.owner as Player;
                var realDeltaPos = collidePlayer.moveCtr.AddPos(new Vector3(m_deltaPos.x, 0, 0));
                m_deltaPos.x = realDeltaPos.x;
                //velocity.z = 0;
                mCollidePlayer = collidePlayer;
            }
            else
            {
                m_deltaPos.x = hit.point.x - (m_minAABB.rect.position.x - m_minAABB.rect.width / 2);
                m_velocity.z = 0;
            }

        }

        protected override void HandleHitRight(RaycastHit hit)
        {

            if (hit.collider.owner != null && hit.collider.owner is Player)
            {
                m_deltaPos.x = m_deltaPos.x / 2;
                Player collidePlayer = hit.collider.owner as Player;
                var realDeltaPos = collidePlayer.moveCtr.AddPos(new Vector3(m_deltaPos.x, 0, 0));
                m_deltaPos.x = realDeltaPos.x;
                //velocity.z = 0;
                mCollidePlayer = collidePlayer;
            }
            else
            {
                m_deltaPos.x = hit.point.x - (m_minAABB.rect.position.x + m_minAABB.rect.width / 2);
                m_velocity.z = 0;
            }
        }
*/
       

    }
}
