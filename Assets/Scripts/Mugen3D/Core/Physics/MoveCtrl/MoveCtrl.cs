using System;
using System.Collections;
using System.Collections.Generic;
using Math = Mugen3D.Core.Math;
using Vector = Mugen3D.Core.Vector;
using Number = Mugen3D.Core.Number;

namespace Mugen3D.Core
{

    public class MoveCtrl
    {
        public Vector gravity { get { return m_gravity; } }
        public Vector velocity { get { return m_velocity; } }
        public bool isOnGround { get; protected set; }
        public bool justOnGround { get; protected set; }

        protected Unit m_owner;
        protected Vector m_velocity = Vector.zero;
        protected Vector m_acceleratedVelocity = Vector.zero;
        protected Vector m_deltaPos = Vector.zero;
        protected Vector m_gravity = new Vector(0, -16, 0);
        protected Number mass = 70;
        protected Vector m_externalForce = Vector.zero;
        protected Number groundFrictionFactor = 3;

        private static Number TINY = new Number(2) / new Number(10);

        public MoveCtrl(Unit unit) {
            isOnGround = true;
            justOnGround = false;
            m_owner = unit;
        }

        public virtual void Update(Number deltaTime)
        {      
            if (justOnGround)
            {
                isOnGround = true;
                justOnGround = false;
            }
            if (m_owner.GetPhysicsType() == PhysicsType.S || m_owner.GetPhysicsType() == PhysicsType.C)
            {
                m_acceleratedVelocity = (m_gravity.magnitude * mass + m_externalForce.y) / mass * groundFrictionFactor * (-velocity.normalized) + m_externalForce / mass;
            }
            else if (m_owner.GetPhysicsType() == PhysicsType.A)
            {
                m_acceleratedVelocity = m_gravity + m_externalForce / mass;
            }
            else
            {
                m_acceleratedVelocity = Vector.zero;
            }
            m_velocity += deltaTime * m_acceleratedVelocity;
            if (m_owner.GetPhysicsType() == PhysicsType.S || m_owner.GetPhysicsType() == PhysicsType.C)
            {
                m_velocity = StabilizeVel(velocity);
            }
            AddPos(m_velocity * deltaTime);
        }

        private Vector StabilizeVel(Vector v)
        {
            Number x, y, z;
            x = Math.Abs(v.x) < TINY ? 0 : v.x;
            y = Math.Abs(v.y) < TINY ? 0 : v.y;
            z = Math.Abs(v.z) < TINY ? 0 : v.z;
            return new Vector(x, y, z);
        }

        public Vector AddPos(Vector deltaPos)
        {
            m_deltaPos = deltaPos;
            PushTest();
            var pos = m_owner.position;
            pos += m_deltaPos;
            m_owner.SetPosition(pos);
            return m_deltaPos;
        }

        public void VelSet(Number velx, Number vely)
        {
            this.m_velocity = new Vector(velx * m_owner.GetFacing(), vely, 0);
        }

        public void VelAdd(Number deltaX, Number deltaY)
        {
            this.m_velocity.x += deltaX * m_owner.GetFacing();
            this.m_velocity.y += deltaY;
        }

        public void PosSet(Number x, Number y)
        {
            m_owner.SetPosition(new Vector(x, y, 0));
        }

        public void PosAdd(Number deltaX, Number deltaY)
        {
            var pos = m_owner.position;
            pos.x += deltaX;
            pos.y += deltaY;
            m_owner.SetPosition(pos);
        }

        public void SetGravity(Number x, Number y)
        {
            m_gravity = new Vector(x, y, 0);
        }

        public void SetForce(Vector force)
        {
            m_externalForce = force;
        }

        protected virtual void PushTest()
        {
            
        }

    }
}
