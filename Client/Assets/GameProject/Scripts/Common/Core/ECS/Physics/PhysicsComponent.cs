using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// <summary>
    /// 负责约束物体的速度，加速度，避免相互穿越等情况，使之呈现物理合理性
    /// </summary>
    class PhysicsComponent: ComponentBase
    {
        public PhysicsType PhysicsType { get { return m_physicsType; } }
        public Number Mass { get { return m_mass; } }

        private PhysicsType m_physicsType;
        private Vector m_gravity = new Vector(0, -16);
        private Number m_mass;
        private Vector m_externalForce = Vector.zero;
        private Number m_groundFrictionFactor = 3;


        public void Update(Number deltaTime)
        {
            Vector acceleratedVelocity = Vector.zero;
            if (m_physicsType == PhysicsType.Stand || m_physicsType == PhysicsType.Crouch)
            {
                //acceleratedVelocity = (m_gravity.magnitude * m_mass + m_externalForce.y) / m_mass * m_groundFrictionFactor * (-m_velocity.normalized) + m_externalForce / m_mass;
            }
            else if (m_physicsType == PhysicsType.Air)
            {
                acceleratedVelocity = m_gravity + m_externalForce / m_mass;
            }
            else
            {
                acceleratedVelocity = Vector.zero;
            }
            //m_velocity += Time.deltaTime * acceleratedVelocity;
            //var deltaPos = m_velocity * deltaTime;
            //m_position += deltaPos;
        }

        public void SetGravity(Number x, Number y)
        {
            m_gravity = new Vector(x, y);
        }

        public void SetForce(Vector force)
        {
            m_externalForce = force;
        }

    }
}
