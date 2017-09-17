using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{

    public enum PhysicsType
    {
        None,
        Stand,
        Croch,
        Air,
    }

    public class MoveCtr
    {
        public PhysicsType type = PhysicsType.Stand;
        public Vector3 gravity = new Vector3(0, -10f, 0);
        public Vector3 velocity = Vector3.zero;
        public Vector3 acceleratedVelocity = Vector3.zero;
        public float groundFrictionFactor = 0.75f;
        public float mass = 70f;
        public Vector3 mExternalForce = Vector3.zero;
        Transform target;

        public MoveCtr(Transform t) {
            target = t;
        }

        public void Update()
        {
            if (type == PhysicsType.Stand || type == PhysicsType.Croch)
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
                AddPos(velocity * Time.deltaTime);
            }
        }

        void UpdateAir()
        {
            acceleratedVelocity = gravity;
            velocity += Time.deltaTime * acceleratedVelocity;
            AddPos(velocity * Time.deltaTime);
        }

        void AddPos(Vector3 deltaPos)
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
