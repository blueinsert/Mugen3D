using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D{
public class RectCollider : Collider {

    public static readonly int FULL = -1;
    public static readonly int LEFT = 1 << 0;
    public static readonly int RIGHT = 1 << 1;
    public static readonly int TOP = 1 << 2;
    public static readonly int DOWN = 1 << 3;

    public int side { get; private set; }

    public Vector2 position;
    public float width;
    public float height;

    public RectCollider(Vector2 position, float width, float height, int side)
    {
        this.position = position;
        this.width = width;
        this.height = height;
        this.side = side;
    }

    public Vector3[] GetVertex()
    {
        Vector3 leftBottom = new Vector3(0, position.y - height / 2, position.x - width / 2);
        Vector3 leftTop = new Vector3(0, position.y + height / 2, position.x - width / 2);
        Vector3 rightTop = new Vector3(0, position.y + height / 2, position.x + width / 2);
        Vector3 rightBottom = new Vector3(0, position.y - height / 2, position.x + width / 2);
        return new Vector3[] { leftBottom,leftTop,rightTop,rightBottom};
    }
}
}
