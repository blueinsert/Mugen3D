using System;
using System.Collections;
using System.Collections.Generic;
using FixPointMath;

namespace bluebean.Mugen3D.Core
{

    public class MoveCtrl
    {
        public Unit owner { get; private set; }
        public ComplexCollider collider { get; protected set; }

        protected Vector m_gravity = new Vector(0, -16);
        protected Number groundFrictionFactor = 3;

        public Vector velocity { get { return m_velocity; } }
        public Vector position { get { return m_position; } }

        public Number mass { get; private set; }
        private Vector m_externalForce = Vector.zero;
        private Vector m_acceleratedVelocity = Vector.zero;
        private Vector m_velocity;
        private Vector m_deltaPos;
        private Vector m_position = Vector.zero;

        public int facing { get { return owner.GetFacing(); } }

        public MoveCtrl(Unit unit) {
            owner = unit;
            collider = new ComplexCollider(unit);
            mass = 70;
        }

        public virtual void Update()
        { 
            if (owner.GetPhysicsType() == PhysicsType.S || owner.GetPhysicsType() == PhysicsType.C)
            {
                m_acceleratedVelocity = (m_gravity.magnitude * mass + m_externalForce.y) / mass * groundFrictionFactor * (-m_velocity.normalized) + m_externalForce / mass;
            }
            else if (owner.GetPhysicsType() == PhysicsType.A)
            {
                m_acceleratedVelocity = m_gravity + m_externalForce / mass;
            }
            else
            {
                m_acceleratedVelocity = Vector.zero;
            }
            m_velocity += Time.deltaTime * m_acceleratedVelocity;
            m_deltaPos = m_velocity * Time.deltaTime;
            m_position += m_deltaPos;
        }

        public void VelSet(Number velx, Number vely)
        {
            this.m_velocity = new Vector(velx * owner.GetFacing(), vely);
        }

        public void VelAdd(Number deltaX, Number deltaY)
        {
            this.m_velocity.x += deltaX * owner.GetFacing();
            this.m_velocity.y += deltaY;
        }

        public void SetGravity(Number x, Number y)
        {
            m_gravity = new Vector(x, y);
        }

        public void SetForce(Vector force)
        {
            m_externalForce = force;
        }

        public void PosSet(Number x, Number y)
        {
            m_position.x = x;
            m_position.y = y;
        }

        public void PosSet(Vector pos)
        {
            m_position.x = pos.x;
            m_position.y = pos.y;
        }

        public void PosAdd(Number deltaX, Number deltaY)
        {
            m_position.x += deltaX;
            m_position.y += deltaY;
        }

        public void PosAdd(Vector deltaPos)
        {
            m_position.x += deltaPos.x;
            m_position.y += deltaPos.y;
        }

        public bool JustOnGround()
        {
           return m_velocity.y* Time.deltaTime + m_position.y < owner.world.config.stageConfig.borderYMin;
        }

        /*
        public bool IntersectWithWorldBound()
        {
            var worldConfig = m_owner.world.config;
            return position.x < worldConfig.stageConfig.borderXMin || position.x > worldConfig.stageConfig.borderXMax || position.y < worldConfig.stageConfig.borderYMin || position.y > worldConfig.stageConfig.borderYMax;
        }

        public bool IntersectWithScreenBound()
        {
            Rect screenBound = m_owner.world.cameraController.viewPort;
            Number playerWidth = new Number(5) / new Number(10);
            return position.x < screenBound.xMin + playerWidth || position.x > screenBound.xMax - playerWidth;
        }
        */

        public void ApplyPosition()
        {
            this.owner.SetPosition(this.m_position);
        }
    }
}
