﻿using System;
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

        protected const float SAFE_DISTANCE = 0.001f;

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
            if (m_owner.status.physicsType == PhysicsType.S || m_owner.status.physicsType == PhysicsType.C)
            {
                m_acceleratedVelocity = -m_gravity.magnitude * groundFrictionFactor * velocity.normalized;
            }
            else if (m_owner.status.physicsType == PhysicsType.A)
            {
                m_acceleratedVelocity = m_gravity;
            }
            m_velocity += Time.deltaTime * m_acceleratedVelocity;
            if (m_owner.status.physicsType == PhysicsType.S || m_owner.status.physicsType == PhysicsType.C)
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
            float playerWidth = 0.7f;
            var pos = this.m_owner.transform.transform.position;
            var newPos = pos + m_deltaPos;
            var viewportRect = World.Instance.camCtl.viewportRect;
            if (newPos.x < viewportRect.position.x - viewportRect.width / 2 + playerWidth)
            {
                newPos.x = viewportRect.position.x - viewportRect.width / 2 + playerWidth;
            }
            if (newPos.x > viewportRect.position.x + viewportRect.width / 2 - playerWidth)
            {
                newPos.x = viewportRect.position.x + viewportRect.width / 2 - playerWidth;
            }
           if (newPos.x < World.Instance.config.borderXMin)
           {
               newPos.x = World.Instance.config.borderXMin;
           }
           if (newPos.x > World.Instance.config.borderXMax)
           {
               newPos.x = World.Instance.config.borderXMax;
           }
           if (newPos.y < World.Instance.config.borderYMin)
           {
               justOnGround = true;
               newPos.y = World.Instance.config.borderYMin;
           }
           m_deltaPos = newPos - pos;
           var enemy = World.Instance.teamInfo.GetEnemy(m_owner as Character);
           foreach (var clsn in m_owner.animCtr.curActionFrame.clsns)
           {
               foreach (var clsn2 in enemy.animCtr.curActionFrame.clsns)
               {
                   if (clsn.type == 1 && clsn2.type == 1)
                   {
                       Rect rect1 = new Rect(new Vector2(clsn.x1, clsn.y1) , new Vector2(clsn.x2, clsn.y2));
                       rect1.position += new Vector2(m_owner.transform.position.x, m_owner.transform.position.y);
                       Rect rect2 = new Rect(new Vector2(clsn2.x1, clsn2.y1), new Vector2(clsn2.x2, clsn2.y2));
                       rect2.position += new Vector2(enemy.transform.position.x, enemy.transform.position.y);
                       Vector2 minDeltaPos;
                       if (PhysicsUtils.RectRectTest(rect1, m_deltaPos, rect2, out minDeltaPos))
                       {
                           if (minDeltaPos.magnitude < m_deltaPos.magnitude)
                           {
                               m_deltaPos = minDeltaPos;
                           }
                       }   
                   }
               }
           }
        }

    }
}
