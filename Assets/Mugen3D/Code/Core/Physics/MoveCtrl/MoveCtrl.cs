using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{

    public abstract class MoveCtrl
    {
        public Vector3 gravity
        {
            get
            {
                return m_gravity;
            }
        }

        public Vector3 velocity {
            get
            {
                return m_velocity;
            }
        }
       
        public bool isOnGround = true;
        public bool justOnGround = false;

        protected Unit m_owner;

        protected Vector3 m_velocity = Vector3.zero;
        protected Vector3 m_acceleratedVelocity = Vector3.zero;
        protected Vector3 m_deltaPos = Vector3.zero;
        protected Vector3 m_gravity = new Vector3(0, -10f, 0);
        protected float mass = 70f;
        protected Vector3 mExternalForce = Vector3.zero;
        protected float groundFrictionFactor = 3f;

        protected RectCollider mCollider;

        public MoveCtrl(Unit unit) {
            m_owner = unit;
        }

        public virtual void Update()
        {
            if (justOnGround)
            {
                isOnGround = true;
                justOnGround = false;
            }
            if (m_owner.status.physicsType == PhysicsType.Stand || m_owner.status.physicsType == PhysicsType.Crouch)
            {
                m_acceleratedVelocity = -m_gravity.magnitude * groundFrictionFactor * velocity.normalized;
            }
            else if (m_owner.status.physicsType == PhysicsType.Air)
            {
                m_acceleratedVelocity = m_gravity;
            }
            m_velocity += Time.deltaTime * m_acceleratedVelocity;
            if (m_owner.status.physicsType == PhysicsType.Stand || m_owner.status.physicsType == PhysicsType.Crouch)
            {
                m_velocity = StabilizeVel(velocity);
            }
            AddPos(velocity * Time.deltaTime);
        }

        private Vector3 StabilizeVel(Vector3 v)
        {
            float x, y, z;
            x = Mathf.Abs(v.x) < 0.2 ? 0: v.x;
            y = Mathf.Abs(v.y) < 0.2 ? 0 : v.y;
            z = Mathf.Abs(v.z) < 0.2 ? 0 : v.z;
            return new Vector3(x, y, z);
        }

        public Vector3 AddPos(Vector3 deltaPos)
        {
            m_deltaPos = deltaPos;
            CollideTest();
            Vector3 pos = m_owner.transform.position;
            pos += m_deltaPos;
            m_owner.transform.position = pos;
            return m_deltaPos;
        }

        public void VelSet(float velx, float vely, float velz = 0)
        {
            this.m_velocity = new Vector3(velx, vely, velz);
        }

        public void VelAdd(float deltaX, float deltaY, float deltaZ = 0)
        {
            this.m_velocity.x += deltaX;
            this.m_velocity.y += deltaY;
            this.m_velocity.z += deltaZ;
        }

        public void PosSet(float x, float y, float z = 0)
        {
            m_owner.transform.transform.position = new Vector3(x, y, z);
        }

        public void PosAdd(float deltaX, float deltaY, float deltaZ = 0)
        {
            var pos = m_owner.transform.transform.position;
            pos.x += deltaX;
            pos.y += deltaY;
            pos.z += deltaZ;
            m_owner.transform.transform.position = pos;
        }

        public void SetGravity(float x, float y, float z)
        {
            m_gravity = new Vector3(x, y, z);
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
            var hit = World.Instance.collisionWorld.Raycast2DAxisAligned(rayStart1, "up", rayLength);
            if (hit != null)
                hits.Add(hit);
            hit = World.Instance.collisionWorld.Raycast2DAxisAligned(rayStart2, "up", rayLength);
            if (hit != null)
                hits.Add(hit);
            if (hits.Count != 0)
            {
                hits.Sort((x, y) => { return x.point.y.CompareTo(y.point.y); });
                hit = hits[0];
                HandleHitUp(hit);
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
            var hit = World.Instance.collisionWorld.Raycast2DAxisAligned(rayStart1, "down", rayLength);
            if (hit != null)
                hits.Add(hit);
            hit = World.Instance.collisionWorld.Raycast2DAxisAligned(rayStart2, "down", rayLength);
            if (hit != null)
                hits.Add(hit);
            if (hits.Count != 0)
            {
                hits.Sort((x, y) => { return y.point.y.CompareTo(x.point.y); });
                hit = hits[0];
                HandleHitBelow(hit);
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
            var hit = World.Instance.collisionWorld.Raycast2DAxisAligned(rayStart1, "left", rayLength);
            if (hit != null)
                hits.Add(hit);
            hit = World.Instance.collisionWorld.Raycast2DAxisAligned(rayStart2, "left", rayLength);
            if (hit != null)
                hits.Add(hit);
            if (hits.Count != 0)
            {
                hits.Sort((x, y) => { return y.point.x.CompareTo(x.point.x); });
                hit = hits[0];
                HandleHitLeft(hit);
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
            var hit = World.Instance.collisionWorld.Raycast2DAxisAligned(rayStart1, "right", rayLength);
            if (hit != null)
                hits.Add(hit);
            hit = World.Instance.collisionWorld.Raycast2DAxisAligned(rayStart2, "right", rayLength);
            if (hit != null)
                hits.Add(hit);
            if (hits.Count != 0)
            {
                hits.Sort((x, y) => { return x.point.x.CompareTo(y.point.x); });
                hit = hits[0];
                HandleHitRight(hit);
            }
        }

        protected abstract void HandleHitUp(RaycastHit hit);
        protected abstract void HandleHitBelow(RaycastHit hit);
        protected abstract void HandleHitLeft(RaycastHit hit);
        protected abstract void HandleHitRight(RaycastHit hit);
    }
}
