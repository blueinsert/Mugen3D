using System.Collections;
using System.Collections.Generic;
using FixPointMath;

namespace bluebean.Mugen3D.Core
{
    /// <summary>
    /// 物理状态
    /// </summary>
    public enum PhysicsType
    {
        None = 0,
        /// <summary>
        /// 站立
        /// </summary>
        Stand,
        /// <summary>
        /// 蹲着
        /// </summary>
        Crouch,
        /// <summary>
        /// 空中
        /// </summary>
        Air,
    }

    public class MoveComponent : ComponentBase
    {
        public Vector Positon { get { return m_position; } }
        public int Facing { get { return m_facing; } }
        public Number Mass { get { return m_mass; } }
        public PhysicsType PhysicsType { get { return m_physicsType; } }

        private PhysicsType m_physicsType;
        private Vector m_position;
        private Vector m_velocity;
        private int m_facing;
        private Vector m_gravity = new Vector(0, -16);
        private Number m_mass;
        private Vector m_externalForce = Vector.zero;
        private Number m_groundFrictionFactor = 3;

        public void Update(Number deltaTime)
        {
            Vector acceleratedVelocity = Vector.zero;
            if (m_physicsType == PhysicsType.Stand || m_physicsType == PhysicsType.Crouch)
            {
                acceleratedVelocity = (m_gravity.magnitude * m_mass + m_externalForce.y) / m_mass * m_groundFrictionFactor * (-m_velocity.normalized) + m_externalForce / m_mass;
            }
            else if (m_physicsType == PhysicsType.Air)
            {
                acceleratedVelocity = m_gravity + m_externalForce / m_mass;
            }
            else
            {
                acceleratedVelocity = Vector.zero;
            }
            m_velocity += Time.deltaTime * acceleratedVelocity;
            var deltaPos = m_velocity * deltaTime;
            m_position += deltaPos;
        }

        public void VelSet(Number velx, Number vely)
        {
            this.m_velocity = new Vector(velx * m_facing, vely);
        }

        public void VelAdd(Number deltaX, Number deltaY)
        {
            this.m_velocity.x += deltaX * m_facing;
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

        public void PosAdd(Number deltaX, Number deltaY)
        {
            m_position.x += deltaX;
            m_position.y += deltaY;
        }

        public void PosAdd(Vector deltaPos)
        {
            m_position += deltaPos;
        }

        public void ChangeFacing(int facing)
        {
            m_facing = facing;
        }
    }
}
