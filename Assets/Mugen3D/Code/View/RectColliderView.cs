using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    public class RectColliderView : MonoBehaviour, Collideable
    {
        public Vector2 halfSize;
        public Vector2 offset;
        public bool top = true;
        public bool down = true;
        public bool left = true;
        public bool right = true;

        private RectCollider Collider()
        {
            int side = 0;
            if (top)
                side += Rect.TOP;
            if (down)
                side += Rect.DOWN;
            if (left)
                side += Rect.LEFT;
            if (right)
                side += Rect.RIGHT;
            Vector2 pos = new Vector2(transform.position.x, transform.position.y) + offset;
            var collider = new RectCollider(new Rect(pos, halfSize.x*2, halfSize.y*2), side);
            collider.id = -1;
            return collider;
        }

        private void OnDrawGizmos()
        {
            Collider().DrawGizmos();
        }

        Collider[] Collideable.GetCollider()
        {
            return new Collider[] { this.Collider() };
        }
    }
}
