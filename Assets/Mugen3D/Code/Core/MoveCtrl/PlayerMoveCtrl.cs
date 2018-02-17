using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D
{
    public class PlayerMoveCtrl : MoveCtrl
    {
        protected RectCollider mCollider;
        protected Player mCollidePlayer;

        public PlayerMoveCtrl(Unit u):base(u)
        {

        }

        protected override void BeforeAddPos() {
            CollideTest();
        }

        protected override void AfterAddPos() {
            IntersectingTest();
        }

        private void IntersectingTest()
        {
            if (mCollidePlayer == null)
                return;
            RectCollider selfCollider = m_owner.GetComponent<DecisionBoxManager>().GetCollider();
            RectCollider otherCollider = mCollidePlayer.GetComponent<DecisionBoxManager>().GetCollider();
            if (ColliderUtils.RectRectTest(selfCollider, otherCollider))
            {
                float deltaX = (selfCollider.rect.width + otherCollider.rect.width) / 2 - Mathf.Abs(selfCollider.rect.position.x - otherCollider.rect.position.x);
                float centerX = (selfCollider.rect.position.x + otherCollider.rect.position.x) / 2;
                m_owner.moveCtr.AddPos(new Vector3(deltaX / 2 * (centerX > selfCollider.rect.position.x ? -1 : 1), 0, 0));
                mCollidePlayer.moveCtr.AddPos(new Vector3(deltaX / 2 * (centerX > otherCollider.rect.position.x ? -1 : 1), 0, 0));
            }
        }

        private void CollideTest()
        {
            mCollider = m_owner.GetComponent<DecisionBoxManager>().GetCollider();
            if (m_owner.status.physicsType == PhysicsType.Stand || m_owner.status.physicsType == PhysicsType.Crouch)
            {
                CastRaysLeft();
                CastRaysRight();
            }
            else if (m_owner.status.physicsType == PhysicsType.Air)
            {
                CastRaysLeft();
                CastRaysRight();
                CastRaysUp();
                CastRaysBelow();
            }
        }

        private void CastRaysUp()
        {
            if (m_deltaPos.y <= 0)
                return;
            var rayLength = mCollider.rect.height / 2 + m_deltaPos.y;
            var rayStart1 = new Vector2(mCollider.rect.position.x + mCollider.rect.width / 2, mCollider.rect.position.y);
            var rayStart2 = new Vector2(mCollider.rect.position.x - mCollider.rect.width / 2, mCollider.rect.position.y);
            var hits = new List<RaycastHit>();
            var hit = CollisionWorld.Instance.Raycast2DAxisAligned(rayStart1, "up", rayLength);
            if (hit != null)
                hits.Add(hit);
            hit = CollisionWorld.Instance.Raycast2DAxisAligned(rayStart2, "up", rayLength);
            if (hit != null)
                hits.Add(hit);
            if (hits.Count != 0)
            {
                hits.Sort((x, y) => { return x.point.y.CompareTo(y.point.y); });
                hit = hits[0];
                HandleHitUp(hit);
            }
        }

        private void HandleHitUp(RaycastHit hit)
        {
            if (hit.collider.owner != null && hit.collider.owner is Player)
            {
            }
            else
            {
                m_deltaPos.y = hit.point.y - (mCollider.rect.position.y + mCollider.rect.height / 2);
                m_velocity.y = 0;
            }
        }

        private void CastRaysBelow()
        {
            if (m_deltaPos.y >= 0)
                return;
            var rayLength = mCollider.rect.height / 2 - m_deltaPos.y;
            var rayStart1 = new Vector2(mCollider.rect.position.x + mCollider.rect.width / 2, mCollider.rect.position.y);
            var rayStart2 = new Vector2(mCollider.rect.position.x - mCollider.rect.width / 2, mCollider.rect.position.y);
            var hits = new List<RaycastHit>();
            var hit = CollisionWorld.Instance.Raycast2DAxisAligned(rayStart1, "down", rayLength);
            if (hit != null)
                hits.Add(hit);
            hit = CollisionWorld.Instance.Raycast2DAxisAligned(rayStart2, "down", rayLength);
            if (hit != null)
                hits.Add(hit);
            if (hits.Count != 0)
            {
                hits.Sort((x, y) => { return y.point.y.CompareTo(x.point.y); });
                hit = hits[0];
                HandleHitBelow(hit);
            }
        }

        private void HandleHitBelow(RaycastHit hit)
        {
            if (hit.collider.owner != null && hit.collider.owner is Player)
            {
                /*
                RectCollider hitCollider = hit.collider as RectCollider;
                float colliderPosX = mCollider.position.x + mDeltaPos.z;
                float movementX = (mCollider.width + hitCollider.width) / 2 - Mathf.Abs(hitCollider.position.x - colliderPosX);
                float sign = hitCollider.position.x > colliderPosX ? 1 : -1;
                Player collidePlayer = hit.collider.owner as Player;
                collidePlayer.moveCtr.AddPos(new Vector3(0, 0, movementX*sign));
              */
            }
            else
            {
                m_deltaPos.y = -hit.point.y + (mCollider.rect.position.y - mCollider.rect.height / 2);
                m_velocity.y = 0;
                m_owner.status.physicsType = PhysicsType.Stand;
                this.justOnGround = true;
            }
        }

        private void CastRaysLeft()
        {
            if (m_deltaPos.x >= 0)
                return;
            var rayLength = mCollider.rect.width / 2 - m_deltaPos.x;
            var rayStart1 = new Vector2(mCollider.rect.position.x, mCollider.rect.position.y + mCollider.rect.height / 2);
            var rayStart2 = new Vector2(mCollider.rect.position.x, mCollider.rect.position.y - mCollider.rect.height / 2);
            var hits = new List<RaycastHit>();
            var hit = CollisionWorld.Instance.Raycast2DAxisAligned(rayStart1, "left", rayLength);
            if (hit != null)
                hits.Add(hit);
            hit = CollisionWorld.Instance.Raycast2DAxisAligned(rayStart2, "left", rayLength);
            if (hit != null)
                hits.Add(hit);
            if (hits.Count != 0)
            {
                hits.Sort((x, y) => { return y.point.x.CompareTo(x.point.x); });
                hit = hits[0];
                HandleHitLeft(hit);
            }
        }

        private void HandleHitLeft(RaycastHit hit)
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
                m_deltaPos.x = hit.point.x - (mCollider.rect.position.x - mCollider.rect.width / 2);
                m_velocity.z = 0;
            }

        }

        private void CastRaysRight()
        {
            if (m_deltaPos.x <= 0)
                return;
            var rayLength = mCollider.rect.width / 2 + m_deltaPos.x;
            var rayStart1 = new Vector2(mCollider.rect.position.x, mCollider.rect.position.y + mCollider.rect.height / 2);
            var rayStart2 = new Vector2(mCollider.rect.position.x, mCollider.rect.position.y - mCollider.rect.height / 2);
            var hits = new List<RaycastHit>();
            var hit = CollisionWorld.Instance.Raycast2DAxisAligned(rayStart1, "right", rayLength);
            if (hit != null)
                hits.Add(hit);
            hit = CollisionWorld.Instance.Raycast2DAxisAligned(rayStart2, "right", rayLength);
            if (hit != null)
                hits.Add(hit);
            if (hits.Count != 0)
            {
                hits.Sort((x, y) => { return x.point.x.CompareTo(y.point.x); });
                hit = hits[0];
                HandleHitRight(hit);
            }
        }

        private void HandleHitRight(RaycastHit hit)
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
                m_deltaPos.x = hit.point.x - (mCollider.rect.position.x + mCollider.rect.width / 2);
                m_velocity.z = 0;
            }
        }

    }
}
