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

        public bool RayCast(Ray ray, out List<RaycastHit> hitResults, Filter filter = null)
        {
            hitResults = new List<RaycastHit>();
            foreach (var c in m_colliders)
            {
                if (!c.interactable || (filter != null && filter(c)))
                    continue;
                RaycastHit hit;
                if (c.IsHit(ray, out hit))
                {
                    hit.collider = c;
                    hitResults.Add(hit);
                }
            }
            bool isHit = false;
            if (hitResults.Count != 0)
            {
                isHit = true;
                hitResults.Sort((a, b) => { return a.distance.CompareTo(b.distance); });
            }
            return isHit;
        }

        public bool ABBCast(ABB abb, Vector3 rayDir, float rayLength, out List<RaycastHit> hitResults, Filter filter = null)
        {
            hitResults = new List<RaycastHit>(); 
            int faceSide = 0;
            if (rayDir.x > 0)
            {
                faceSide = faceSide + BoundBox.RIGHT;
            }
            else if (rayDir.x < 0)
            {
                faceSide = faceSide + BoundBox.LEFT;
            }
            if (rayDir.y > 0)
            {
                faceSide += BoundBox.TOP;
            }
            else if (rayDir.y < 0)
            {
                faceSide += BoundBox.DOWN;
            }
            var vertexes = abb.GetVertexes(faceSide);
            var center = abb.GetCenter();
            foreach (var vertex in vertexes)
            {
                List<RaycastHit> result;
                if (RayCast(new Ray() { start = vertex, end = vertex + rayDir * rayLength }, out result, filter))
                {
                    hitResults.AddRange(result);
                }
            }
            Dictionary<Collider, List<RaycastHit>> classfityResult = new Dictionary<Collider, List<RaycastHit>>();
            foreach (var hit in hitResults)
            {
                if (!classfityResult.ContainsKey(hit.collider))
                {
                    classfityResult[hit.collider] = new List<RaycastHit>();
                }
                classfityResult[hit.collider].Add(hit);
            }
            foreach (var kv in classfityResult)
            {
                kv.Value.Sort((a, b) => { return a.distance.CompareTo(b.distance); });
            }
            hitResults.Clear();
            foreach (var kv in classfityResult)
            {
                hitResults.Add(kv.Value[0]);
            }
            bool isHit = false;
            if (hitResults.Count != 0)
            {
                isHit = true;
                foreach (var hit in hitResults)
                {
                    //calc normal
                    if (hit.collider is ABBCollider)
                    {
                        var hitABB = (hit.collider as ABBCollider).abb;
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
                        else if (maxX1 <= minX2)
                        {
                            normal.x = -1;
                        }
                        else
                        {
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
                        hit.normal = normal;
                    }
                }
            }
            return isHit;       
        }

    }//class
}//namespace