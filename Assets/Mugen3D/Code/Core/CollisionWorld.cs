using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D
{
    public class CollisionWorld
    {
        private CollisionWorld() { }
        private static CollisionWorld mInstance;
        public static CollisionWorld Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new CollisionWorld();
                }
                return mInstance;
            }
        }

        private List<Collideable> m_colliders = new List<Collideable>();

        public List<Collider> GetColliders()
        {
            List<Collider> colliders = new List<Collider>();
            foreach (var collideable in m_colliders)
            {
                colliders.AddRange(collideable.GetColliders());
            }
            return colliders;
        }

        public void Clear()
        {
            m_colliders.Clear();
        }

        public void AddCollideable(Collideable c)
        {
            m_colliders.Add(c);
        }

        public void RemoveCollideable(Collideable c)
        {
            m_colliders.Remove(c);
        }

        public int GetCollideableNum()
        {
            return m_colliders.Count;
        }

        public RaycastHit Raycast2DAxisAligned(Vector2 origin, string dir, float distance)
        {
            List<RaycastHit> raycastResults = new List<RaycastHit>();
            DoRayCast2DAxisAligned(origin, dir, distance, raycastResults, 1);
            if (raycastResults.Count > 0)
            {
                return raycastResults[0];
            }
            return null;
        }

        private void DoRayCast2DAxisAligned(Vector2 origin, string dir , float distance, List<RaycastHit> results, int max)
        {
            Vector2 vss = Vector2.zero;
            Vector2 vse = Vector2.zero;
            Vector2 hss = Vector2.zero;
            Vector2 hse = Vector2.zero;
            if (dir == "left" || dir == "right")
            {
                hss = hse = origin;
                if (dir == "right")
                {
                    hse.x += distance;
                }
                else
                {
                    hss.x -= distance;
                }
            }
            else
            {
                vss = vse = origin;
                if (dir ==  "up")
                {
                    vse.y += distance;
                }
                else
                {
                    vss.y -= distance;
                }
            }
            foreach (var e in GetColliders())
            { 
                bool oblique = false;
                if (e is RectCollider)
                {
                    var c = e as RectCollider;
                    if (dir =="right" && (c.side & Rect.LEFT) == 0) continue;
                    if (dir == "left" && (c.side & Rect.RIGHT) == 0) continue;
                    if (dir == "up" && (c.side & Rect.DOWN) == 0) continue;
                    if (dir == "down" && (c.side & Rect.TOP) == 0) continue;
                    var p = c.rect.position;
                    var s = new Vector2(c.rect.width / 2, c.rect.height / 2);
                    if (dir == "left" || dir == "right")
                    {
                        vss.x = vse.x = (dir == "right" ? p.x - s.x : p.x + s.x);
                        vss.y = p.y - s.y;
                        vse.y = p.y + s.y;
                    }
                    else
                    {
                        hss.y = hse.y = (dir == "up" ? p.y - s.y : p.y + s.y);
                        hss.x = p.x - s.x;
                        hse.x = p.x + s.x;
                    }
                }

                Vector2 point = Vector2.zero;
                bool hit = oblique ? ColliderUtils.SegmentIntersectionTest(hss, hse, vss, vse, ref point) : ColliderUtils.SegmentIntersectionAxisAlignedTest(hss, hse, vss, vse, ref point);
                if (hit)
                {
                    Debug.DrawLine(point, point + new Vector2(2,0), Color.red, 0.025f);
                    var result = new RaycastHit();
                    result.point = point;
                    result.collider = e;
                    results.Add(result);
                    if (results.Count >= max) break;
                }
            }//foreach collider
        }
    }//class
}//namespace