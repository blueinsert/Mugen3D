using System.Collections;
using System.Collections.Generic;

namespace Mugen3D.Core
{
    public class ContactInfo
    {
        public Vector recoverDir;
        public Number depth;
    }

    public class CollideRect
    {
        private MoveCtrl m_moveCtrl;
        public Vector offset;
        public Number width;
        public Number height;

        public CollideRect(MoveCtrl moveCtrl, Vector offset, Number width, Number height)
        {
            this.m_moveCtrl = moveCtrl;
            this.offset = offset;
            this.width = width;
            this.height = height;
        }

        public Rect GetRect()
        {
            Rect rect = new Rect(m_moveCtrl.position + new Vector(offset.x * m_moveCtrl.facing, offset.y, offset.z), width, height);
            return rect;
        }

        public bool IsOverlap(CollideRect rect)
        {
            return GetRect().IsOverlap(rect.GetRect());
        }

    }

    public class Collider
    {
        private MoveCtrl m_owner;

        private List<CollideRect> m_attackClsns = new List<CollideRect>();
        private List<CollideRect> m_defenceClsns = new List<CollideRect>();
        private List<CollideRect> m_collideClsns = new List<CollideRect>();

        public Collider(MoveCtrl owner)
        {
            this.m_owner = owner;
        }

        public void SetCollider(List<Clsn> clsns)
        {
            m_attackClsns.Clear();
            m_defenceClsns.Clear();
            m_collideClsns.Clear();
            foreach (var clsn in clsns)
            {
                switch (clsn.type)
                {
                    case 1:
                        m_defenceClsns.Add(new CollideRect(this.m_owner, new Vector((clsn.x1 + clsn.x2) / 2, (clsn.y1 + clsn.y2) / 2, 0), Math.Abs(clsn.x1 - clsn.x2), Math.Abs(clsn.y1 - clsn.y2)));
                        m_collideClsns.Add(new CollideRect(this.m_owner, new Vector((clsn.x1 + clsn.x2) / 2, (clsn.y1 + clsn.y2) / 2, 0), Math.Abs(clsn.x1 - clsn.x2), Math.Abs(clsn.y1 - clsn.y2)));
                        break;
                    case 2:
                        m_attackClsns.Add(new CollideRect(this.m_owner, new Vector((clsn.x1 + clsn.x2) / 2, (clsn.y1 + clsn.y2) / 2, 0), Math.Abs(clsn.x1 - clsn.x2), Math.Abs(clsn.y1 - clsn.y2)));
                        break;
                }
            }
        }

      

        public bool IsIntersect(Collider c, out ContactInfo contactInfo)
        {
            contactInfo = null;
            for (int i = 0; i < m_collideClsns.Count; i++)     
            {
                var rect1 = m_collideClsns[i];
                for (int j = 0; j < c.m_collideClsns.Count; j++) {
                    var rect2 = c.m_collideClsns[j];
                    if (rect1.IsOverlap(rect2))
                    {
                        Vector dir = new Vector(rect1.GetRect().position.x > rect2.GetRect().position.x ? 1 : -1, 0, 0);
                        Number depth = (rect1.width + rect2.width) / 2 - Math.Abs(rect1.GetRect().position.x - rect2.GetRect().position.x);
                        contactInfo = new ContactInfo() { recoverDir = dir, depth = depth/2 };
                        return true;
                    }
                }                     
            }
            return false;
        }

    }
}