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

        public bool RayCast(Ray ray, out RaycastHit hitResult, Filter filter = null)
        {
            hitResult = null;
            List<RaycastHit> results = new List<RaycastHit>();
            foreach (var c in m_colliders)
            {
                if (!c.interactable || (filter != null && filter(c)))
                    continue;
                RaycastHit hit;
                if (c.IsHit(ray, out hit))
                {
                    hit.collider = c;
                    results.Add(hit);
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

        public bool ABBCast(ABB abb, Vector3 rayDir, float rayLength, out RaycastHit hitResult, Filter filter = null)
        {
            hitResult = null;
            List<RaycastHit> results = new List<RaycastHit>();
            //var center = obb.GetCenter();
            foreach (var vertex in abb.GetVertexArray())
            {
                //var v = vertex - (center - vertex).normalized * 0.001f;
                RaycastHit hit;
                if (RayCast(new Ray() { start = vertex, end = vertex + rayDir * rayLength }, out hit, filter))
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