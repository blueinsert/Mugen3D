using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mugen3D;
public class RaycastTest2 : MonoBehaviour
{
    public Transform rayStart;
    public Transform rayEnd;

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(rayStart.position, 0.1f);
        Gizmos.DrawSphere(rayEnd.position, 0.1f);
        Gizmos.DrawLine(rayStart.position, rayEnd.position);
        List<Mugen3D.RaycastHit> hitResults;
        if (World.Instance.collisionWorld.RayCast(new Mugen3D.Ray() { start = rayStart.position, end = rayEnd.position }, out hitResults))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(hitResults[0].point, 0.1f);
        }
    }
}
