using System.Collections;
using System.Collections.Generic;
using FixPointMath;

namespace bluebean.Mugen3D.Core
{
    public class ContactInfo
    {
        public Vector recoverDir;
        public Number depth;
    }

    public enum ColliderType
    {
        RectCollider = 1,
        ComplexCollider,
    }

    public class Collider
    {
        public ColliderType type { get; protected set; }

        public virtual bool IsIntersect(Collider c, out ContactInfo contactInfo)
        {
            contactInfo = null;
            return PhysicsUtils.IsIntersect(this, c, out contactInfo);
        }

    }

    public class RectCollider : Collider
    {
        public Vector Position
        {
            get
            {
                return new Vector(m_offset.x * m_facing, m_offset.y) + m_position;
            }
        }

        public Number xMin
        {
            get
            {
                return m_position.x - m_width / 2;
            }
        }

        public Number xMax
        {
            get
            {
                return m_position.x + m_width / 2;
            }
        }

        public Number yMin
        {
            get
            {
                return m_position.y - m_height / 2;
            }
        }

        public Number yMax
        {
            get
            {
                return m_position.y + m_height / 2;
            }
        }

        public Vector LeftUp
        {
            get
            {
                return m_position + new Vector(-m_width / 2, m_height / 2);
            }
        }

        public Vector RightUp
        {
            get
            {
                return m_position + new Vector(m_width / 2, m_height / 2);
            }
        }

        public Vector RightDown
        {
            get
            {
                return m_position + new Vector(m_width / 2, -m_height / 2);
            }
        }

        public Vector LeftDown
        {
            get
            {
                return m_position + new Vector(-m_width / 2, -m_height / 2);
            }
        }

        private Vector m_offset;
        private Number m_width;
        private Number m_height;
        private Vector m_position;
        private int m_facing;

        public RectCollider()
        {
            this.type = ColliderType.RectCollider;
        }

        public void Update(Vector position, int facing, Vector offset, Number width, Number height)
        {
            m_position = position;
            m_facing = facing;
            m_offset = offset;
            m_width = width;
            height = m_height;
        }
        
    }

    public class ComplexCollider : Collider
    {
        public RectCollider[] AttackClsns
        {
            get
            {
                return m_attackClsnArray;
            }
        }
        public RectCollider[] DefenceClsns
        {
            get
            {
                return m_defenceClsnArray;
            }
        }
        public RectCollider[] CollideClsns
        {
            get
            {
                return m_collideClsnArray;
            }
        }

        public int AttackClsnsLength { get { return m_attackClsnsLength; } }
        public int DefenceClsnsLength { get { return m_defenceClsnsLength; } }
        public int CollideClsnsLength { get { return m_collideClsnsLength; } }

        private static readonly int MAX_ATTACK_CLSN_NUM = 10;
        private static readonly int MAX_DEFENCE_CLSN_NUM = 10;
        private static readonly int MAX_COLLIDE_CLSN_NUM = 10;
        private RectCollider[] m_attackClsnArray;
        private RectCollider[] m_defenceClsnArray;
        private RectCollider[] m_collideClsnArray;
        private int m_attackClsnsLength;
        private int m_defenceClsnsLength;
        private int m_collideClsnsLength;

        public ComplexCollider()
        {
            this.type = ColliderType.ComplexCollider;
            m_attackClsnArray = new RectCollider[MAX_ATTACK_CLSN_NUM];
            m_defenceClsnArray = new RectCollider[MAX_DEFENCE_CLSN_NUM];
            m_collideClsnArray = new RectCollider[MAX_COLLIDE_CLSN_NUM];
            for (int i = 0; i < MAX_ATTACK_CLSN_NUM; i++)
            {
                m_attackClsnArray[i] = new RectCollider();
            }
            for (int i = 0; i < MAX_DEFENCE_CLSN_NUM; i++)
            {
                m_defenceClsnArray[i] = new RectCollider();
            }
            for (int i = 0; i < MAX_COLLIDE_CLSN_NUM; i++)
            {
                m_collideClsnArray[i] = new RectCollider();
            }
        }

        public void Update(List<Clsn> clsns, Vector position, int facing)
        {
            m_attackClsnsLength = 0;
            m_defenceClsnsLength = 0;
            m_collideClsnsLength = 0;
            foreach (var clsn in clsns)
            {
                Vector clsnCenter = new Vector((clsn.x1 + clsn.x2) / 2, (clsn.y1 + clsn.y2) / 2);
                Number width = Math.Abs(clsn.x1 - clsn.x2);
                Number height = Math.Abs(clsn.y1 - clsn.y2);
                switch (clsn.type)
                {
                    case 1:
                        var rectCollider = m_defenceClsnArray[m_defenceClsnsLength];
                        rectCollider.Update(position, facing, clsnCenter, width, height);
                        m_defenceClsnArray[m_defenceClsnsLength++] = rectCollider;
                        break;
                    case 2:
                        rectCollider = m_attackClsnArray[m_attackClsnsLength];
                        rectCollider.Update(position, facing, clsnCenter, width, height);
                        m_attackClsnArray[m_attackClsnsLength++] = rectCollider;
                        break;
                    case 3:
                        rectCollider = m_collideClsnArray[m_collideClsnsLength];
                        rectCollider.Update(position, facing, clsnCenter, width, height);
                        m_collideClsnArray[m_collideClsnsLength++] = rectCollider;
                        break;
                }
            }
        }

    }

    public class CollideComponent : ComponentBase
    {
        public ComplexCollider Collider { get { return m_collider; } set { m_collider = value; } }
        private ComplexCollider m_collider = new ComplexCollider();

        public void Update(List<Clsn> clsns, Vector position, int facing)
        {
            m_collider.Update(clsns, position, facing);
        }
    }
}
