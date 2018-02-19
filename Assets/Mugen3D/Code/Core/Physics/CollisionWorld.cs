﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D
{
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

        private float GetNearestDist(Vector3 p, out Collider collider)
        {
            collider = null;
            float minDist = float.MaxValue;
            foreach (var c in m_colliders)
            {
                float dist = PhysicsUtils.DistToGeometry(c.GetGeometry(), p);
                if (dist < minDist)
                {
                    minDist = Mathf.Min(dist, minDist);
                    collider = c;
                }
            }
            return minDist;
        }

        public bool RayCast(Vector3 rayStart, Vector3 rayDir, float rayLength, out RaycastHit hitResult)
        {
            hitResult = new RaycastHit();
            bool isHit = false;
            int maxIterNum = 10;
            float goLength = 0;
            for (int i = 0; i < maxIterNum; i++)
            {
                Collider nearestCollider;
                float minDist = GetNearestDist(rayStart + goLength * rayDir, out nearestCollider);
                if (minDist <= 0.001f)
                {
                    hitResult = new RaycastHit { collider = nearestCollider, distance = goLength, point = rayStart + rayDir * goLength };
                    isHit = true;
                    break;
                }
                goLength += minDist;
            }
            return isHit;
        }

        public bool BoxCast(Vector3[] vertexes, Vector3 rayDir, float rayLength, out RaycastHit hitResult)
        {
            hitResult = new RaycastHit();
            List<RaycastHit> hitResults = new List<RaycastHit>();
            foreach (var vertex in vertexes)
            {   RaycastHit r;
                if (RayCast(vertex, rayDir, rayLength, out r))
                {
                    hitResults.Add(r);
                }
            }
            bool isHit = false;
            if (hitResults.Count > 0)
            {
                isHit = true;
                hitResults.Sort((a, b) => { return a.distance.CompareTo(b.distance); });
                hitResult = hitResults[0];
            }
            else
            {
                isHit = false;
            }
            return isHit;
        }
    }//class
}//namespace