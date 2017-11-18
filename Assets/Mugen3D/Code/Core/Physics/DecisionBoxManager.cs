using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{

    public class DecisionBoxManager : MonoBehaviour
    {
        private Dictionary<HitBoxLocation, HitBox> mAttackBoxDic = new Dictionary<HitBoxLocation, HitBox>();

        public List<HitBox> attactBoxes = new List<HitBox>();
        public List<DefenceBox> defenceBoxes = new List<DefenceBox>();
        public List<CollideBox> collideBoxes = new List<CollideBox>();

        void Start()
        {
            foreach (var b in attactBoxes)
            {
                mAttackBoxDic[b.location] = b;
            }
        }

        public HitBox GetHitBox(HitBoxLocation type)
        {
            if (mAttackBoxDic.ContainsKey(type))
            {
                return mAttackBoxDic[type];
            }
            return null;
        }

        public Box2D GetCollideBox()
        {
            List<Vector3> vertexes = new List<Vector3>();

            float ymin = float.MaxValue;
            float ymax = float.MinValue;
            float zmin = float.MaxValue;
            float zmax = float.MinValue;
            foreach (var box in collideBoxes)
            {
                foreach (var v in box.cuboid.GetVertexArray())
                {
                    if (v.y > ymax)
                        ymax = v.y;
                    if (v.y < ymin)
                        ymin = v.y;
                    if (v.z > zmax)
                        zmax = v.z;
                    if (v.z < zmin)
                        zmin = v.z;
                }
            }
            Box2D r = new Box2D(new Vector2((zmin + zmax) / 2, (ymin + ymax) / 2), zmax - zmin, ymax - ymin);
            return r;
        }

        void Update()
        {

        }

        void OnDrawGizmos()
        {
            foreach (var b in attactBoxes)
            {
                DrawDecisionBox(b, Color.red);
            }
            foreach (var b in defenceBoxes)
            {
                DrawDecisionBox(b, Color.blue);
            }
            foreach (var b in collideBoxes)
            {
                DrawDecisionBox(b, Color.green);
            }
            DrawRect(GetCollideBox().GetVertex(), Color.black);
        }

        void DrawRect(Vector3[] points, Color c)
        {
            Gizmos.color = c;
            for (int i = 0; i < points.Length - 1; i++)
            {
                Gizmos.DrawLine(points[i], points[i + 1]);
            }
            Gizmos.DrawLine(points[points.Length - 1], points[0]);
        }

        void DrawDecisionBox(DecisionBox box, Color c)
        {
            if (!box.visualable)
                return;
            Gizmos.color = c;
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
    }//class

}//namespace
