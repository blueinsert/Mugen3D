using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mugen3D;

public class RectColliderView : MonoBehaviour {

    public enum RectOrient
    {
        X = 1,
        Y,
        Z,
    }

    public RectOrient orientation = RectOrient.X;
    public Vector2 halfSize;
    public Vector2 offset;
    public bool top = true;
    public bool down = true;
    public bool left = true;
    public bool right = true;

    public RectCollider GetCollider()
    {
        int side = 0;
        if (top)
            side += RectCollider.TOP;
        if (down)
            side += RectCollider.DOWN;
        if (left)
            side += RectCollider.LEFT;
        if (right)
            side += RectCollider.RIGHT;
        var pos = this.transform.position + realOffset;
        var collider =  new RectCollider(new Vector2(pos.z, pos.y), halfSize.x * 2, halfSize.y * 2, side);
        collider.id = -1;
        return collider;
    }

    private Vector3 realOffset {
        get {
            Vector3 result = new Vector3();
            switch (orientation)
            {
                case RectOrient.X:
                    result = new Vector3(0, offset.y, offset.x); break;
                case RectOrient.Y:
                    result = new Vector3(offset.x, 0, offset.y); break;
                case RectOrient.Z:
                    result = new Vector3(offset.x, offset.y, 0); break;
            }
            return result;
        } 
    }

    private Vector3 realHalfSizeX
    {
        get
        {
            Vector3 result = new Vector3();
            switch (orientation)
            {
                case RectOrient.X:
                    result = new Vector3(0, 0, halfSize.x); break;
                case RectOrient.Y:
                    result = new Vector3(halfSize.x, 0, 0); break;
                case RectOrient.Z:
                    result = new Vector3(halfSize.x, 0, 0); break;
            }
            return result;
        } 
    }

    private Vector3 realHalfSizeY
    {
        get
        {
            Vector3 result = new Vector3();
            switch (orientation)
            {
                case RectOrient.X:
                    result = new Vector3(0, halfSize.y, 0); break;
                case RectOrient.Y:
                    result = new Vector3(0, 0, halfSize.y); break;
                case RectOrient.Z:
                    result = new Vector3(0, halfSize.y, 0); break;
            }
            return result;
        }
    }

    private Vector3 topLeft
    {
        get
        {
            return transform.position + realOffset - realHalfSizeX + realHalfSizeY;
        }
    }

    private Vector3 topRight
    {
        get
        {
            return transform.position + realOffset + realHalfSizeX + realHalfSizeY;
        }
    }

    private Vector3 bottomLeft
    {
        get
        {
            return transform.position + realOffset - realHalfSizeX - realHalfSizeY;
        }
    }

    private Vector3 bottomRight
    {
        get
        {
            return transform.position + realOffset + realHalfSizeX - realHalfSizeY;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        if (this.left) Gizmos.DrawLine(bottomLeft, topLeft);
        if (this.right) Gizmos.DrawLine(bottomRight, topRight);
        if (this.top) Gizmos.DrawLine(topLeft, topRight);
        if (this.down) Gizmos.DrawLine(bottomLeft, bottomRight);
    }

    private void Update() {
     
    }

}
