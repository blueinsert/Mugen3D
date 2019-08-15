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

    public class Collider {
        public ColliderType type { get; protected set; }
        public virtual bool IsIntersect(Collider c, out ContactInfo contactInfo)
        {
            contactInfo = null;
            return PhysicsUtils.IsIntersect(this, c, out contactInfo);
        }

    }

    public class RectCollider : Collider
    {
        private Unit owner;
        public Vector offset;
        public Number width;
        public Number height;

        public RectCollider(Unit owner)
        {
            this.owner = owner;
            this.type = ColliderType.RectCollider;
        }

        public Vector position {
            get {
                return new Vector(offset.x * owner.GetFacing(), offset.y) + owner.Position;
            }
        }

        public Number xMin
        {
            get
            {
                return position.x - width / 2;
            }
        }

        public Number xMax
        {
            get
            {
                return position.x + width / 2;
            }
        }

        public Number yMin
        {
            get
            {
                return position.y - height / 2;
            }
        }

        public Number yMax
        {
            get
            {
                return position.y + height / 2;
            }
        }

        public Vector LeftUp
        {
            get
            {
                return position + new Vector(-width / 2, height / 2);
            }
        }

        public Vector RightUp
        {
            get
            {
                return position + new Vector(width / 2, height / 2);
            }
        }

        public Vector RightDown
        {
            get
            {
                return position + new Vector(width / 2, -height / 2);
            }
        }

        public Vector LeftDown
        {
            get
            {
                return position + new Vector(-width / 2, -height / 2);
            }
        }
    }

    public class ComplexCollider : Collider
    {
        private Unit owner;
        public List<RectCollider> attackClsns
        {
            get {
                return m_attackClsns;
            }
        }
        public List<RectCollider> defenceClsns
        {
            get
            {
                return m_defenceClsns;
            }
        }
        public List<RectCollider> collideClsns
        {
            get
            {
                return m_collideClsns;
            }
        }
        private static readonly int MAX_ATTACK_CLSN_NUM = 10;
        private static readonly int MAX_DEFENCE_CLSN_NUM = 10;
        private static readonly int MAX_COLLIDE_CLSN_NUM = 10;
        private List<RectCollider> m_attackClsns = new List<RectCollider>(MAX_ATTACK_CLSN_NUM);
        private List<RectCollider> m_defenceClsns = new List<RectCollider>(MAX_DEFENCE_CLSN_NUM);
        private List<RectCollider> m_collideClsns = new List<RectCollider>(MAX_COLLIDE_CLSN_NUM);
        public int attackClsnsLength { get; private set; }
        public int defenceClsnsLength { get; private set; }
        public int collideClsnsLength { get; private set; }

        public ComplexCollider(Unit owner)
        {
            this.owner = owner;
            for(int i = 0; i < MAX_ATTACK_CLSN_NUM; i++)
            {
                this.m_attackClsns.Add(new RectCollider(owner));
            }
            for (int i = 0; i < MAX_DEFENCE_CLSN_NUM; i++)
            {
                this.m_defenceClsns.Add(new RectCollider(owner));
            }
            for (int i = 0; i < MAX_COLLIDE_CLSN_NUM; i++)
            {
                this.m_collideClsns.Add(new RectCollider(owner));
            }
            this.type = ColliderType.ComplexCollider;
        }

        public void SetCollider(List<Clsn> clsns)
        {
            attackClsnsLength = 0;
            defenceClsnsLength = 0;
            collideClsnsLength = 0;
            foreach (var clsn in clsns)
            {
                switch (clsn.type)
                {
                    case 1:
                        var rectCollider = m_defenceClsns[defenceClsnsLength];
                        rectCollider.offset = new Vector((clsn.x1 + clsn.x2) / 2, (clsn.y1 + clsn.y2) / 2);
                        rectCollider.width = Math.Abs(clsn.x1 - clsn.x2);
                        rectCollider.height = Math.Abs(clsn.y1 - clsn.y2);
                        m_defenceClsns[defenceClsnsLength++] = rectCollider;
                        break;
                    case 2:
                        rectCollider = m_attackClsns[attackClsnsLength];
                        rectCollider.offset = new Vector((clsn.x1 + clsn.x2) / 2, (clsn.y1 + clsn.y2) / 2);
                        rectCollider.width = Math.Abs(clsn.x1 - clsn.x2);
                        rectCollider.height = Math.Abs(clsn.y1 - clsn.y2);
                        m_attackClsns[attackClsnsLength++] = rectCollider;
                        break;
                    case 3:
                        rectCollider = m_collideClsns[collideClsnsLength];
                        rectCollider.offset = new Vector((clsn.x1 + clsn.x2) / 2, (clsn.y1 + clsn.y2) / 2);
                        rectCollider.width = Math.Abs(clsn.x1 - clsn.x2);
                        rectCollider.height = Math.Abs(clsn.y1 - clsn.y2);
                        m_collideClsns[collideClsnsLength++] = rectCollider;
                        break;
                }
            }
        }
    
    }
}