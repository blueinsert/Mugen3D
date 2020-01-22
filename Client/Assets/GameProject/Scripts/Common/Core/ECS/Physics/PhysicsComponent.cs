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
        /// <summary>
        /// 重力常量
        /// </summary>
        public static readonly Number G = new Number(-16);
        /// <summary>
        /// 摩擦系数
        /// </summary>
        public static readonly Number Friction = 3;

        public Vector ExternalForce { get { return m_externalForce; } }
        public PhysicsType PhysicsType { get { return m_physicsType; } }
        public Number Mass { get { return m_mass; } }

        private PhysicsType m_physicsType = PhysicsType.None;
        private Number m_mass = new Number(70);
        private Vector m_externalForce = Vector.zero;

        public void SetPhysicsType(PhysicsType physicsType)
        {
            m_physicsType = physicsType;
        }
       
        public void SetMass(Number mass)
        {
            m_mass = mass;
        }

        public void SetForce(Vector force)
        {
            m_externalForce = force;
        }

    }
}
