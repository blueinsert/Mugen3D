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

        protected ABBCollider m_minBBCollider;

        protected const float SAFE_DISTANCE = 0.01f;

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
            AfterAddPos();
        }

        protected virtual void AfterAddPos() { }

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

        protected virtual void OnHitCollider(RaycastHit hitResult)
        {
           
        }

        private void CollideTest()
        {
            if (Mathf.Abs(m_deltaPos.x) < 0.001 && Mathf.Abs(m_deltaPos.y) < 0.001)
                return;
            if (m_owner.status.physicsType != PhysicsType.Air)
            {
                m_deltaPos.y = 0;
            }
            m_minBBCollider = m_owner.decisionBoxes.GetMinBBCollider();
            
            RaycastHit hitResult = null; 

            if (World.Instance.collisionWorld.ABBCast(m_minBBCollider.abb, m_deltaPos.normalized, m_deltaPos.magnitude + SAFE_DISTANCE, out hitResult, (c) => { return c == m_minBBCollider; }))
            {
                Debug.DrawLine(hitResult.point, hitResult.point - m_deltaPos.normalized * hitResult.distance, Color.red);
                Debug.DrawLine(hitResult.point, hitResult.point, Color.red);
                hitResult.distance -= SAFE_DISTANCE;
                OnHitCollider(hitResult);
            }
          
        }
    }
}
