using System.Collections;
using System.Collections.Generic;
using FixPointMath;

namespace bluebean.Mugen3D.Core
{
    /// <summary>
    /// 根据受力情况，更新物体的位置和速度，不考虑物理接触和碰撞
    /// </summary>
    public class MoveComponent : ComponentBase
    {
        public Vector Velocity { get { return m_velocity; } }
        public Vector Acceler { get { return m_acceleratedVelocity; } }
        //public int Facing { get { return m_facing; } }

        private Vector m_acceleratedVelocity = Vector.zero;
        private Vector m_velocity;

        public void AccelerateSet(Number x, Number y)
        {
            m_acceleratedVelocity.x = x;
            m_acceleratedVelocity.y = y;
        }

        public void AccelerateSet(Vector a)
        {
            m_acceleratedVelocity = a;
        }

        public void VelSet(Number velx, Number vely)
        {
            this.m_velocity = new Vector(velx, vely);
        }



        public void VelSet(Vector vel)
        {
            this.m_velocity = vel;
        }

        public void VelAdd(Number deltaX, Number deltaY)
        {
            this.m_velocity.x += deltaX;
            this.m_velocity.y += deltaY;
        }

        public void VelAdd(Vector velDelta)
        {
            this.m_velocity.x += velDelta.x;
            this.m_velocity.y += velDelta.y;
        }

    }
}
