using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{

    public enum PhysicsType
    {
        None = -1,
        Stand,
        Crouch,
        Air,    
    }

    public class MoveCtr
    {
        public PhysicsType type = PhysicsType.Stand;
        public readonly Vector3 gravity = new Vector3(0, -30f, 0);
        public Vector3 velocity = Vector3.zero;
        public Vector3 acceleratedVelocity = Vector3.zero;
        public float groundFrictionFactor = 0.75f;
        public float mass = 70f;
        public Vector3 mExternalForce = Vector3.zero;
        public bool pushTestOn = true;
        public bool isOnGround = true;
        public bool justOnGround = false;
        Transform target;

        public MoveCtr(Transform t) {
            target = t;
        }

        public void Update()
        {
            if (justOnGround)
            {
                isOnGround = true;
                justOnGround = false;
            }
            if (type == PhysicsType.Stand || type == PhysicsType.Crouch)
            {
                UpdateGround();
            }
            else if (type == PhysicsType.Air)
            {
                UpdateAir();
            }
        }

        void UpdateGround()
        {
            if (velocity != Vector3.zero)
            {
                acceleratedVelocity = -gravity.magnitude * groundFrictionFactor * velocity.normalized;
                velocity += Time.deltaTime * acceleratedVelocity;
                velocity = StabilizeVel(velocity);
                Vector3 deltaPos = velocity * Time.deltaTime;
                if (pushTestOn)
                {
                    deltaPos = PushTest(velocity * Time.deltaTime);
                }
                CastRaysLeft(ref deltaPos);
                CastRaysRight(ref deltaPos);
                AddPos(deltaPos);
            }
        }

        void UpdateAir()
        {
            acceleratedVelocity = gravity;
            velocity += Time.deltaTime * acceleratedVelocity;
            //velocity = StabilizeVel(velocity);
            Vector3 deltaPos = velocity * Time.deltaTime;
            if (pushTestOn)
            {
                deltaPos = PushTest(velocity * Time.deltaTime);
            }
            CastRaysLeft(ref deltaPos);
            CastRaysRight(ref deltaPos);
            CastRaysUp(ref deltaPos);
            CastRaysBelow(ref deltaPos);
            AddPos(deltaPos);
        }

        private void CastRaysUp(ref Vector3 deltaPos)
        {
            if (deltaPos.y <= 0)
                return;
            var collider = target.GetComponent<DecisionBoxManager>().GetCollideBox();
            var rayLength = collider.height / 2 + deltaPos.y;
            var rayStart1 = new Vector2(collider.center.x + collider.width / 2, collider.center.y);
            var rayStart2 = new Vector2(collider.center.x - collider.width / 2, collider.center.y);
            var hits = new List<RaycastHit>();
            var hit = CollisionWorld.Instance.Raycast2DAxisAligned(rayStart1, "up", rayLength);
            if (hit != null)
                hits.Add(hit);
            hit = CollisionWorld.Instance.Raycast2DAxisAligned(rayStart2, "up", rayLength);
            if (hit != null)
                hits.Add(hit);
            if (hits.Count != 0)
            {
                Debug.Log("hit ceiling");
                hits.Sort((x, y) => { return x.point.y.CompareTo(y.point.y); });
                hit = hits[0];
                deltaPos.y = hit.point.y - (collider.center.y + collider.height / 2);
                velocity.y = 0;
            }
        }

        private void CastRaysBelow(ref Vector3 deltaPos)
        {
            if (deltaPos.y >= 0)
                return;
            var collider = target.GetComponent<DecisionBoxManager>().GetCollideBox();
            var rayLength = collider.height / 2 - deltaPos.y;
            var rayStart1 = new Vector2(collider.center.x + collider.width / 2, collider.center.y);
            var rayStart2 = new Vector2(collider.center.x - collider.width / 2, collider.center.y);
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
                deltaPos.y = -hit.point.y + (collider.center.y - collider.height / 2);
                velocity.y = 0;
                this.type = PhysicsType.Stand;
                this.justOnGround = true;
            }
        }

        private void CastRaysLeft(ref Vector3 deltaPos)
        {
            if (deltaPos.z >= 0)
                return;
            var collider = target.GetComponent<DecisionBoxManager>().GetCollideBox();
            var rayLength = collider.width / 2 - deltaPos.z;
            var rayStart1 = new Vector2(collider.center.x, collider.center.y + collider.height / 2);
            var rayStart2 = new Vector2(collider.center.x, collider.center.y - collider.height / 2);
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
                deltaPos.z = hit.point.x - (collider.center.x - collider.width / 2);
                velocity.z = 0;
            }
        }

        private void CastRaysRight(ref Vector3 deltaPos)
        {
            if (deltaPos.z <= 0)
                return;
            var collider =  target.GetComponent<DecisionBoxManager>().GetCollideBox();
            var rayLength = collider.width / 2 + deltaPos.z;
            var rayStart1 = new Vector2(collider.center.x, collider.center.y + collider.height / 2);
            var rayStart2 = new Vector2(collider.center.x, collider.center.y - collider.height / 2);
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
                deltaPos.z = hit.point.x - (collider.center.x + collider.width / 2);
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

        public void AddPos(Vector3 deltaPos)
        {    
            Vector3 pos = target.position;
            pos += deltaPos;
            target.position = pos;
        }

        public void VelSet(float velx, float vely)
        {
            this.velocity = new Vector3(0, vely, velx);
        }

        public void VelAdd(float deltaX, float deltaY)
        {
             this.velocity.y += deltaY;
             this.velocity.z += deltaX;
        }

        public void PosSet(float x, float y)
        {
            this.target.transform.position = new Vector3(0, y, x);
        }

        public void PosAdd(float deltaX, float deltaY)
        {
            var pos = this.target.transform.position;
            pos.y += deltaY;
            pos.z += deltaX;
            this.target.transform.position = pos;
        }

        public void SetPhysicsType(PhysicsType type)
        {
            this.type = type;
        }
    }
}
