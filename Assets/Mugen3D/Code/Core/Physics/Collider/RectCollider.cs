using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D{
public class RectCollider : Collider {

    public int side { get; private set; }
    public Rect rect;

    public RectCollider(Rect rect, int side)
    {
        this.rect = rect;
        this.side = side;
    }

    public void DrawGizmos()
    {
        rect.DrawGizmos(Color.green, side);
    }
}
}
