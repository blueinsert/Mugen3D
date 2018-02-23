using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D
{
    [ExecuteInEditMode]
    public class DecisionBoxes : MonoBehaviour
    {
        public Color attactBoxColor;
        public Color defenceBoxColor;
        public Color colliderBoxColor;
        public List<HitBox> attackBoxes = new List<HitBox>();
        public List<OBBCollider> defenceBoxes = new List<OBBCollider>();
        public List<OBBCollider> colliderBoxes = new List<OBBCollider>();
        public OBBCollider minBB;
        private List<OBBCollider> m_allCollider = new List<OBBCollider>();
        private Dictionary<HitPart, HitBox> m_attactBoxDic = new Dictionary<HitPart, HitBox>();
        private Unit m_owner;

        public void SetOwner(Unit u)
        {
            m_owner = u;
        }

        public void Init()
        {
            m_allCollider.Clear();
            m_allCollider.Add(minBB);
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

        public OBBCollider GetMinBBCollider()
        {
            return minBB;
        }

        private void UpdateMinBB()
        {
            /*
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
            foreach (var c in colliderBoxes)
            {
                foreach (var vertex in c.obb.GetVertexArray())
                {
                    min.x = Mathf.Min(min.x, vertex.x);
                    min.y = Mathf.Min(min.y, vertex.y);
                    min.z = Mathf.Min(min.z, vertex.z);
                    max.x = Mathf.Max(max.x, vertex.x);
                    max.y = Mathf.Max(max.y, vertex.y);
                    max.z = Mathf.Max(max.z, vertex.z);
                }
            }
            minBB.obb.position = (min + max) / 2;
            minBB.obb.scale = new Vector3(max.x - min.x, max.y - min.y, max.z - min.z);
             */
            //minBB.obb.position = this.transform.position;
        }

        public void Update()
        {
            UpdateMinBB();
        }

    }
}
