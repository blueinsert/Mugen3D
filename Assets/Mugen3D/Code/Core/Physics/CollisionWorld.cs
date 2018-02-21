using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D
{
    public delegate bool Filter(Collider col);

    public class CollisionWorld
    {
        private List<Collider> m_colliders = new List<Collider>();

        public void AddCollider(Collider c)
        {
            m_colliders.Add(c);
        }

        public void RemoveCollider(Collider c)
        {
            m_colliders.Remove(c);
        }

        public bool RayCast(Vector3 rayStart, Vector3 rayEnd, out RaycastHit hitResult, Filter filter = null)
        {
            hitResult = null;
            List<RaycastHit> results = new List<RaycastHit>();
            foreach (var c in m_colliders)
            {
                if (!c.interactable || (filter != null && filter(c)))
                    continue;
                Vector3 p;
                float dist;
                if (PhysicsUtils.RayOBBIntersectTest((c as OBBCollider).obb, rayStart, rayEnd, out dist, out p))
                {
                    results.Add(new RaycastHit() {collider = c, distance = dist, point = p});
                }
            }
            bool isHit = false;
            if (results.Count != 0)
            {
                isHit = true;
                results.Sort((a, b) => { return a.distance.CompareTo(b.distance); });
                hitResult = results[0];
            }
            return isHit;
        }

        public bool OBBCast(OBB obb, Vector3 rayDir, float rayLength, out RaycastHit hitResult, Filter filter = null)
        {
            hitResult = null;
            List<RaycastHit> results = new List<RaycastHit>();
            foreach (var vertex in obb.GetVertexArray())
            {   
                RaycastHit hit;
                if (RayCast(vertex, vertex + rayDir * rayLength, out hit, filter))
                {
                    results.Add(hit);
                }
            }
            results.Sort((a, b) => { return a.distance.CompareTo(b.distance); });
            bool isHit = false;
            if (results.Count != 0)
            {
                isHit = true;
                hitResult = results[0];
            }
            return isHit; 
        }
    }//class
}//namespace