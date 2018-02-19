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
        public List<HitBox2> attackBoxes = new List<HitBox2>();
        public List<OBBCollider> defenceBoxes = new List<OBBCollider>();
        public List<OBBCollider> colliderBoxes = new List<OBBCollider>();

        private List<OBBCollider> m_allCollider = new List<OBBCollider>();
        private Dictionary<HitPart, HitBox2> m_attactBoxDic = new Dictionary<HitPart, HitBox2>();

        public void Awake()
        {
            m_attactBoxDic.Clear();
            foreach (var hitbox in attackBoxes)
            {
                if (hitbox != null && hitbox.collider != null)
                {
                    m_attactBoxDic[hitbox.hitPart] = hitbox;
                }
            }
            defenceBoxes.Clear();
            colliderBoxes.Clear();
            m_allCollider.Clear();
            foreach(var c in this.GetComponentsInChildren<OBBCollider>()){
                defenceBoxes.Add(c);
                colliderBoxes.Add(c);
                m_allCollider.Add(c);
            }
        }

        public AABB GetMinAABB()
        {
            return new AABB();
        }

        public void Update()
        {
            
        }


    }
}
