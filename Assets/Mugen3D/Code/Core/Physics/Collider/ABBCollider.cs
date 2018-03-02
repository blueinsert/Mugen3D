using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D
{
    public class ABBCollider : Collider
    {
        public ABB abb;

        public override Geometry GetGeometry()
        {
            return abb;
        }

        public override bool IsHit(Ray ray, out RaycastHit hitResult)
        {
            return PhysicsUtils.RayAABBIntersectTest(abb, ray, out hitResult);
        }

        protected void OnDrawGizmos()
        {
            if (abb == null)
                return;
            Gizmos.color = color;
            List<Vector3> points = abb.GetVertexArray();
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
