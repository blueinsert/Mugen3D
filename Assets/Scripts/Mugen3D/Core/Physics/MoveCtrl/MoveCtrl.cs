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
        protected Unit m_owner;
        public Collider collider { get; protected set; }

        protected Vector m_gravity = new Vector(0, -16, 0);
        protected Number groundFrictionFactor = 3;

        public Vector velocity { get { return m_velocity; } }

        private Number mass = 70;
        private Vector m_externalForce = Vector.zero;
        private Vector m_acceleratedVelocity = Vector.zero;
        private Vector m_velocity;
        private Vector m_deltaPos;
        public Vector position;
        public int facing { get { return m_owner.GetFacing(); } }

        public bool justOnGround = false;

        public MoveCtrl(Unit unit) {
            m_owner = unit;
            collider = new Collider(this);
        }

        public virtual void Update()
        {
            if (justOnGround)
            {
                justOnGround = false;
            }
            if (m_owner.GetPhysicsType() == PhysicsType.S || m_owner.GetPhysicsType() == PhysicsType.C)
            {
                m_acceleratedVelocity = (m_gravity.magnitude * mass + m_externalForce.y) / mass * groundFrictionFactor * (-m_velocity.normalized) + m_externalForce / mass;
            }
            else if (m_owner.GetPhysicsType() == PhysicsType.A)
            {
                m_acceleratedVelocity = m_gravity + m_externalForce / mass;
            }
            else
            {
                m_acceleratedVelocity = Vector.zero;
            }
            m_velocity += Time.deltaTime * m_acceleratedVelocity;
            m_deltaPos = m_velocity * Time.deltaTime;
            position = m_owner.position + m_deltaPos;
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

        public void SetGravity(Number x, Number y)
        {
            m_gravity = new Vector(x, y, 0);
        }

        public void SetForce(Vector force)
        {
            m_externalForce = force;
        }

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

        public void ApplyPosition()
        {
            this.m_owner.SetPosition(this.position);
        }

    }
}
