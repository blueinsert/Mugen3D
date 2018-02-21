using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mugen3D;

public class RayOBBIntersectTest : MonoBehaviour
{

    public Transform rayStart;
    public Transform rayEnd;
    public OBBCollider cuboid;

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(rayStart.position, 0.1f);
        Gizmos.DrawSphere(rayEnd.position, 0.1f);
        Gizmos.DrawLine(rayStart.position, rayEnd.position);

        Vector3 p;
        float dist;
        if (PhysicsUtils.RayOBBIntersectTest(cuboid.obb, rayStart.position, rayEnd.position, out dist, out p))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(p, 0.1f);
        } 
    }
}
