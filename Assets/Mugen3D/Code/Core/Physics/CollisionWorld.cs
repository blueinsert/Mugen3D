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
            var vertexes = abb.GetVertexArray();
            var center = abb.GetCenter();
            foreach (var vertex in vertexes)
            {
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
                //calc normal
                if (hitResult.collider is ABBCollider)
                {
                    var hitABB = (hitResult.collider as ABBCollider).abb;
                    var hitABBCenter = hitABB.GetCenter();
                    var newCenter = center;// +rayDir * hitResult.distance;
                    float minX1 = newCenter.x - abb.size.x / 2;
                    float maxX1 = newCenter.x + abb.size.x / 2;
                    float minY1 = newCenter.y - abb.size.y / 2;
                    float maxY1 = newCenter.y + abb.size.y / 2;
                    float minX2 = hitABBCenter.x - hitABB.size.x / 2;
                    float maxX2 = hitABBCenter.x + hitABB.size.x / 2;
                    float minY2 = hitABBCenter.y - hitABB.size.y / 2;
                    float maxY2 = hitABBCenter.y + hitABB.size.y / 2;
                    Vector3 normal = Vector3.zero;
                    if (maxX2 <= minX1)
                    {
                        normal.x = 1;
                    }
                    else if(maxX1 <= minX2)
                    {
                        normal.x = -1;
                    }else{
                        normal.x = 0;
                    }
                    if (maxY2 <= minY1)
                    {
                        normal.y = 1;
                    }
                    else if (maxY1 <= minY2)
                    {
                        normal.y = -1;
                    }
                    else
                    {
                        normal.y = 0;
                    }
                    hitResult.normal = normal;
                }
            }
            return isHit;       
        }

    }//class
}//namespace