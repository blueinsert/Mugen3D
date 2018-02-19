using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mugen3D;

public class RayCuboidIntersectTest : MonoBehaviour {

    public Transform rayStart;
    public Transform rayEnd;
    public OBBCollider cuboid;

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(rayStart.position, 0.1f);
        Gizmos.DrawSphere(rayEnd.position, 0.1f);
        Gizmos.DrawLine(rayStart.position, rayEnd.position);

        Vector3 nearestPoint;
        PhysicsUtils.ClosestPointAtOBB(cuboid.cuboid, rayStart.position, out nearestPoint);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(nearestPoint, 0.1f);
        float dis = PhysicsUtils.DistToOBB(cuboid.cuboid, rayStart.position);
        //Debug.Log("dis:" + dis);
    }
}
