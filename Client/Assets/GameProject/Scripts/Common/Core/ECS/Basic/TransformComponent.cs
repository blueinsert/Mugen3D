using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FixPointMath;

namespace bluebean.Mugen3D.Core
{
    /// <summary>
    /// 所有实体都具有的组件
    /// </summary>
    class TransformComponent : ComponentBase
    {
        public Vector Position { get { return m_position; } }
        public Number Rotate { get { return m_rotate; } }
        public int Facing { get { return m_facing; } }

        private int m_facing;
        private Number m_rotate;
        private Vector m_position;

        public void SetFacing(int facing)
        {
            m_facing = facing;
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

        public void SetRotate(Number rotate)
        {
            m_rotate = rotate;
        }
    }
}
