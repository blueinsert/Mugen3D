﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{

   

    public class MoveCtr
    {
        private Unit m_owner;
       
        public Vector3 gravity = new Vector3(0, -10f, 0);
        public Vector3 velocity = Vector3.zero;
       
        public float groundFrictionFactor = 3f;
        public float mass = 70f;
        public Vector3 mExternalForce = Vector3.zero;
        public bool pushTestOn = true;
        public bool isOnGround = true;
        public bool justOnGround = false;
        Transform target;

        public Vector3 mAcceleratedVelocity = Vector3.zero;
        private Vector3 mDeltaPos;
        private RectCollider mCollider;
        private Player mCollidePlayer;

        public MoveCtr(Unit unit) {
            target = unit.transform;
            m_owner = unit;
        }

        public void Update()
        {
            if (justOnGround)
            {
                isOnGround = true;
                justOnGround = false;
            }
            if (m_owner.status.physicsType == PhysicsType.Stand || m_owner.status.physicsType == PhysicsType.Crouch)
            {
                mAcceleratedVelocity = -gravity.magnitude * groundFrictionFactor * velocity.normalized;
            }
            else if (m_owner.status.physicsType == PhysicsType.Air)
            {
                mAcceleratedVelocity = gravity;
            }
            velocity += Time.deltaTime * mAcceleratedVelocity;
            if (m_owner.status.physicsType == PhysicsType.Stand || m_owner.status.physicsType == PhysicsType.Crouch)
            {
                velocity = StabilizeVel(velocity);
            }
            AddPos(velocity * Time.deltaTime);
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
                float deltaX = (selfCollider.rect.width + otherCollider.rect.width)/2 - Mathf.Abs(selfCollider.rect.position.x - otherCollider.rect.position.x);
                float centerX = (selfCollider.rect.position.x + otherCollider.rect.position.x) / 2;
                m_owner.moveCtr.AddPos(new Vector3(deltaX / 2 * (centerX > selfCollider.rect.position.x ? -1 : 1), 0, 0));
                mCollidePlayer.moveCtr.AddPos(new Vector3(deltaX / 2 * (centerX > otherCollider.rect.position.x ? -1 : 1), 0, 0));
            }
        }

        private void CollideTest()
        {
            mCollider = target.GetComponent<DecisionBoxManager>().GetCollider();
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
            if (mDeltaPos.y <= 0)
                return;
            var rayLength = mCollider.rect.height / 2 + mDeltaPos.y;
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
                mDeltaPos.y = hit.point.y - (mCollider.rect.position.y + mCollider.rect.height / 2);
                velocity.y = 0;
            }
        }

        private void CastRaysBelow()
        {
            if (mDeltaPos.y >= 0)
                return;
            var rayLength = mCollider.rect.height / 2 - mDeltaPos.y;
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
                mDeltaPos.y = -hit.point.y + (mCollider.rect.position.y - mCollider.rect.height / 2);
                velocity.y = 0;
                m_owner.status.physicsType = PhysicsType.Stand;
                this.justOnGround = true;
            }
        }

        private void CastRaysLeft()
        {
            if (mDeltaPos.x >= 0)
                return;
            var rayLength = mCollider.rect.width / 2 - mDeltaPos.x;
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
                mDeltaPos.x = mDeltaPos.x / 2;
                Player collidePlayer = hit.collider.owner as Player;
                var realDeltaPos = collidePlayer.moveCtr.AddPos(new Vector3(mDeltaPos.x, 0, 0));
                mDeltaPos.x = realDeltaPos.x;
                //velocity.z = 0;
                mCollidePlayer = collidePlayer;
            }
            else
            {
                mDeltaPos.x = hit.point.x - (mCollider.rect.position.x - mCollider.rect.width / 2);
                velocity.z = 0;
            }
           
        }

        private void CastRaysRight()
        {
            if (mDeltaPos.x <= 0)
                return;
            var rayLength = mCollider.rect.width / 2 + mDeltaPos.x;
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
                mDeltaPos.x = mDeltaPos.x / 2;
                Player collidePlayer = hit.collider.owner as Player;
                var realDeltaPos = collidePlayer.moveCtr.AddPos(new Vector3(mDeltaPos.x, 0, 0));
                mDeltaPos.x = realDeltaPos.x;
                //velocity.z = 0;
                mCollidePlayer = collidePlayer;
            }
            else
            {
                mDeltaPos.x = hit.point.x - (mCollider.rect.position.x + mCollider.rect.width / 2);
                velocity.z = 0;
            }
        }

        private Vector3 StabilizeVel(Vector3 v)
        {
            float x, y, z;
            x = Mathf.Abs(v.x) < 0.2 ? 0: v.x;
            y = Mathf.Abs(v.y) < 0.2 ? 0 : v.y;
            z = Mathf.Abs(v.z) < 0.2 ? 0 : v.z;
            return new Vector3(x, y, z);
        }

        /*
        private Vector3 PushTest(Vector3 deltaPos)
        {
            Vector3 realDeltaPos = deltaPos;
            if (target.GetComponent<Player>().facing * deltaPos.z >0)
            {
                Vector2 movment = new Vector2(deltaPos.z, deltaPos.y);
                Box2D box = target.GetComponent<DecisionBoxManager>().GetCollideBox();
                box.center += movment;
                var enemy = TeamMgr.GetEnemy(target.GetComponent<Player>());
                if (enemy != null)
                {
                    Box2D box2 = enemy.GetComponent<DecisionBoxManager>().GetCollideBox();
                    if (ColliderUtils.RectRectTest(box, box2))
                    {
                        movment.x = movment.x / 2;
                        enemy.moveCtr.AddPos(new Vector3(0, 0, movment.x));
                        realDeltaPos = new Vector3(0, movment.y, movment.x);
                    }
                }
            }
            return realDeltaPos;
        }
        */
        public Vector3 AddPos(Vector3 deltaPos)
        {
            mDeltaPos = deltaPos;
            CollideTest();
            Vector3 pos = target.position;
            pos += mDeltaPos;
            target.position = pos;
            return mDeltaPos;
        }

        public void VelSet(float velx, float vely, float velz = 0)
        {
            this.velocity = new Vector3(velx, vely, velz);
        }

        public void VelAdd(float deltaX, float deltaY, float deltaZ = 0)
        {
             this.velocity.x += deltaX;
             this.velocity.y += deltaY;
             this.velocity.z += deltaZ;
        }

        public void PosSet(float x, float y, float z = 0)
        {
            this.target.transform.position = new Vector3(x, y, z);
        }

        public void PosAdd(float deltaX, float deltaY, float deltaZ = 0)
        {
            var pos = this.target.transform.position;
            pos.x += deltaX;
            pos.y += deltaY;
            pos.z += deltaZ;
            this.target.transform.position = pos;
        }
    }
}
