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

        Mugen3D.RaycastHit hitResult;
        if (PhysicsUtils.RayOBBIntersectTest(cuboid.obb, new Mugen3D.Ray() {start = rayStart.position, end = rayEnd.position }, out hitResult))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(hitResult.point, 0.1f);
        } 
    }
}
