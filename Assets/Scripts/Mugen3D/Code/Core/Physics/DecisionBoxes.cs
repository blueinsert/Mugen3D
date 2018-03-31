using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D
{

    public class DecisionBoxes : MonoBehaviour
    {
        public Color attactBoxColor;
        public Color defenceBoxColor;
        public Color colliderBoxColor;

        public List<HitBox> attackBoxes = new List<HitBox>();
        public List<Collider> defenceBoxes = new List<Collider>();
        public ABBCollider collideBox;
        public ABB minBB;

        private List<Collider> m_allCollider = new List<Collider>();
        private Dictionary<HitPart, HitBox> m_attactBoxDic = new Dictionary<HitPart, HitBox>();
        private Unit m_owner;

        public void SetOwner(Unit u)
        {
            m_owner = u;
        }

        public void Init()
        {
            m_allCollider.Clear();
            m_allCollider.Add(collideBox);
            foreach (var c in this.GetComponentsInChildren<OBBCollider>())
            {
                m_allCollider.Add(c);
            }
            foreach (var c in m_allCollider)
            {
                c.SetOwner(this.m_owner);
            }

            m_attactBoxDic.Clear();
            foreach (var hitbox in attackBoxes)
            {
                if (hitbox != null && hitbox.collider != null)
                {
                    m_attactBoxDic[hitbox.hitPart] = hitbox;
                }
            }
        }

        public ABBCollider GetCollideBox()
        {
            return collideBox;
        }

        public HitBox GetHitBox(HitPart part)
        {
            return m_attactBoxDic[part];
        }

        private void UpdateMinBB()
        {  
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
            foreach (var c in m_allCollider)
            {
                foreach (var vertex in c.GetGeometry().GetVertexArray())
                {
                    min.x = Mathf.Min(min.x, vertex.x);
                    min.y = Mathf.Min(min.y, vertex.y);
                    min.z = Mathf.Min(min.z, vertex.z);
                    max.x = Mathf.Max(max.x, vertex.x);
                    max.y = Mathf.Max(max.y, vertex.y);
                    max.z = Mathf.Max(max.z, vertex.z);
                }
            }
            minBB.offset = ((min + max) / 2);
            minBB.size = new Vector3(max.x - min.x, max.y - min.y, max.z - min.z);
        }

        public void Update()
        {
            UpdateMinBB();
        }
 
        protected void OnDrawGizmos()
        {
            if (minBB == null)
                return;
            Gizmos.color = Color.black;
            List<Vector3> points = minBB.GetVertexArray();
            Gizmos.DrawLine(points[0], points[1]);
            Gizmos.DrawLine(points[1], points[2]);
            Gizmos.DrawLine(points[2], points[3]);
            Gizmos.DrawLine(points[3], points[0]);
            Gizmos.DrawLine(points[4], points[5]);
            Gizmos.DrawLine(points[5], points[6]);
            Gizmos.DrawLine(points[6], points[7]);
            Gizmos.DrawLine(points[7], points[4]);
            Gizmos.DrawLine(points[0], points[4]);
            Gizmos.DrawLine(points[1], points[5]);
            Gizmos.DrawLine(points[2], points[6]);
            Gizmos.DrawLine(points[3], points[7]);
        }

    }
}
