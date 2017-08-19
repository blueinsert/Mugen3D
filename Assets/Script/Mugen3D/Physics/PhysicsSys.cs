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

    public class PhysicsSys
    {
        public PhysicsType mPhysicsType = PhysicsType.Stand;
        public PhysicsType Physics { get { return mPhysicsType; } set { mPhysicsType = value; } }
        Vector3 mGravity = new Vector3(0, -10f, 0);
        public Vector3 Gravity { get { return mGravity; } }
        public Vector3 mVelocity = Vector3.zero;
        public Vector3 Velocity { get { return mVelocity; } set { mVelocity = value; } }
        Vector3 mAcceleratedVelocity = Vector3.zero;
        public Vector3 AcceleratedVelocity { get { return mAcceleratedVelocity; } }
        float mGroundFrictionFactor = 0.75f;
        public float GroundFrictionFactor { get { return mGroundFrictionFactor; } }
        float mMass = 70f;
        public float Mass { get { return mMass; } }
        Vector3 mExternalForce = Vector3.zero;
        public Vector3 ExternalForce { get { return mExternalForce; } }
        Transform target;

        public PhysicsSys(Transform t) {
            target = t;
        }

        public void UpdatePhysics()
        {
            if (mPhysicsType == PhysicsType.Stand || mPhysicsType == PhysicsType.Croch)
            {
                UpdateStand();
            }
            else if (mPhysicsType == PhysicsType.Air)
            {
                UpdateAir();
            }
        }

        void UpdateStand()
        {
            if (mVelocity != Vector3.zero)
            {
                //Vector3 frictionForce = -mMass * mGravity.magnitude * mGroundFrictionFactor * mVelocity.normalized;
                mAcceleratedVelocity = -mGravity.magnitude * mGroundFrictionFactor * mVelocity.normalized;
                mVelocity += Time.deltaTime * mAcceleratedVelocity;
                AddPos(mVelocity * Time.deltaTime);
            }
        }

        void UpdateAir()
        {
            mAcceleratedVelocity = mGravity;
            mVelocity += Time.deltaTime * mAcceleratedVelocity;
            AddPos(mVelocity * Time.deltaTime);
        }

        void AddPos(Vector3 deltaPos)
        {
            Vector3 pos = target.position;
            pos += deltaPos;
            target.position = pos;
        }

    }
}
