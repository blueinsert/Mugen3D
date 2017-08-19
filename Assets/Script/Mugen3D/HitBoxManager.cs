using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D{

public class HitBoxManager : MonoBehaviour {
    public List<HitBox> attactBoxes = new List<HitBox>();
    public List<HitBox> defenceBoxes = new List<HitBox>();
    public List<HitBox> collideBoxes = new List<HitBox>();

    void Update()
    {
      
    }

    void OnDrawGizmos()
    {
        foreach (var b in attactBoxes)
        {
            DrawHitBox(b);
        }
        foreach (var b in defenceBoxes)
        {
            DrawHitBox(b);
        }
        foreach (var b in collideBoxes)
        {
            DrawHitBox(b);
        }
    }

    void DrawHitBox(HitBox box) {
        switch (box.type)
        {
            case HitBoxType.Attack:
                Gizmos.color = Color.red;
                break;
            case HitBoxType.Defence:
                Gizmos.color = Color.blue;
                break;
            case HitBoxType.Collide:
                Gizmos.color = Color.black;
                break;
        }
        List<Vector3> points = box.cuboid.GetVertexArray();
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
