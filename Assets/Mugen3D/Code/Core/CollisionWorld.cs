﻿using System.Collections;
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

        private Dictionary<int, Collider> m_dynamicColliders = new Dictionary<int, Collider>();
        private List<Collider> m_staticColliders = new List<Collider>();

        public List<Collider> GetColliders()
        {
            List<Collider> colliders = new List<Collider>();
            colliders.AddRange(m_staticColliders);
            colliders.AddRange(m_dynamicColliders.Values);
            return colliders;
        }

        public void Clear()
        {
            m_dynamicColliders.Clear();
            m_staticColliders.Clear();
        }

        public void AddCollider(Collider c)
        {
            int id = c.id;
            if (id == -1)
            {
                m_staticColliders.Add(c);
            }
            else
            {
                if (m_dynamicColliders.ContainsKey(c.id))
                {
                    Log.Error("Collider was already added into Physics, id:" + c.id);
                }
                m_dynamicColliders.Add(c.id, c);
            }
        }

        public void ReplaceCollider(Collider c)
        {
            if (m_dynamicColliders.ContainsKey(c.id))
            {
                m_dynamicColliders[c.id] = c;
            }
            else
            {
                m_dynamicColliders.Add(c.id, c);
            }
        }

        public void RemoveCollider(Collider c)
        {
            m_dynamicColliders.Remove(c.id);
        }

        public int GetColliderNum()
        {
            return m_dynamicColliders.Count;
        }

        public void Update()
        {
            foreach (var kv in World.Instance.Players)
            {
                ReplaceCollider(kv.Value.GetComponent<DecisionBoxManager>().GetCollider());
            }
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
                //Debug.DrawLine(hss, hse, Color.Blue, 0.02f);
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
                //Debug.DrawLine(vss, vse, Color.Blue, 0.02f);
            }
            foreach (var e in GetColliders())
            {
                /*
                if (!e.entity.isEnabled) continue;
                if ((e.entity.layer & layerMask) == 0) continue;
                if (filter != null && !filter(e)) continue;
                */
                bool oblique = false;
                if (e is RectCollider)
                {
                    var c = e as RectCollider;
                    if (dir =="right" && (c.side & RectCollider.LEFT) == 0) continue;
                    if (dir == "left" && (c.side & RectCollider.RIGHT) == 0) continue;
                    if (dir == "up"  && (c.side & RectCollider.DOWN) == 0) continue;
                    if (dir == "down" && (c.side & RectCollider.TOP) == 0) continue;
                    var p = c.position;
                    var s = new Vector2(c.width / 2, c.height / 2);
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
                    //result.distance = vss.y - hss.y;
                    results.Add(result);

                    if (results.Count >= max) break;
                }
            }//foreach collider
        }
    }//class
}//namespace