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
        Mugen3D.RaycastHit hitResult;
        if (World.Instance.collisionWorld.RayCast(rayStart.position, rayEnd.position, out hitResult))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(hitResult.point, 0.1f);
        }
    }
}
