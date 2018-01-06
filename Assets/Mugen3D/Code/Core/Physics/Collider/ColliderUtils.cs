using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D
{
    public class ColliderUtils
    {
        static Vector3 GetPlaneNormalVector(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            Vector3 v1 = p2 - p1;
            Vector3 v2 = p3 - p1;
            Vector3 normal = Vector3.Cross(v1, v2);
            normal.Normalize();
            return normal;
        }

        public static bool CuboidCuboidTest(Vector3[] C1Points, Vector3[] C2Points)
        {
            Vector3[] normalVectors = new Vector3[6];
            normalVectors[0] = GetPlaneNormalVector(C1Points[0], C1Points[3], C1Points[2]);
            normalVectors[1] = GetPlaneNormalVector(C1Points[0], C1Points[4], C1Points[7]);
            normalVectors[2] = GetPlaneNormalVector(C1Points[3], C1Points[2], C1Points[6]);

            normalVectors[3] = GetPlaneNormalVector(C2Points[0], C2Points[3], C2Points[2]);
            normalVectors[4] = GetPlaneNormalVector(C2Points[0], C2Points[4], C2Points[7]);
            normalVectors[5] = GetPlaneNormalVector(C2Points[3], C2Points[2], C2Points[6]);

            bool isIntersect = true;
            for (int i = 0; i < 6; i++)
            {
                Vector3 normal = normalVectors[i];
                //
                float c1Max = float.MinValue;
                float c1Min = float.MaxValue;
                foreach (var v in C1Points)
                {
                    float projectValue = Vector3.Dot(v,normal);
                    if (projectValue > c1Max)
                    {
                        c1Max = projectValue;
                    }
                    if (projectValue < c1Min)
                    {
                        c1Min = projectValue;
                    }
                }
                //
                float c2Max = float.MinValue;
                float c2Min = float.MaxValue;
                foreach (var v in C2Points)
                {
                    float projectValue = Vector3.Dot(v, normal);
                    if (projectValue > c2Max)
                    {
                        c2Max = projectValue;
                    }
                    if (projectValue < c2Min)
                    {
                        c2Min = projectValue;
                    }
                }
                //
                if (c2Min > c1Max || c1Min > c2Max)
                {
                    isIntersect = false;
                    //Debug.Log("c1Min:" + c1Min + " c1Max" + c1Max + " c2Min:" + c2Min + " c2Max" + c2Max);
                    break;
                }
                
            }//for six separate axis
            return isIntersect;
        }

        public static bool RectRectTest(RectCollider rect1, RectCollider rect2)
        {
            bool isIntersect = false;
            if (Mathf.Abs(rect1.rect.position.x - rect2.rect.position.x) < (rect1.rect.width + rect2.rect.width) / 2 && Mathf.Abs(rect2.rect.position.y - rect1.rect.position.y) < (rect1.rect.height + rect2.rect.height) / 2)
                isIntersect = true;
            return isIntersect;
        }

        public static bool SegmentIntersectionAxisAlignedTest(Vector2 h1, Vector2 h2, Vector2 v1, Vector2 v2, ref Vector2 point)
        {
            Vector2 vss, vse, hss, hse;

            if (v1.y < v2.y)
            {
                vss = v1; vse = v2;
            }
            else
            {
                vss = v2; vse = v1;
            }
            if (h1.x < h2.x)
            {
                hss = h1; hse = h2;
            }
            else
            {
                hss = h2; hse = h1;
            }

            if (hss.x <= vss.x && hse.x >= vss.x && vss.y <= hss.y && vse.y >= hss.y)
            {
                point.x = vss.x;
                point.y = hss.y;
                return true;
            }

            return false;
        }

        public static bool SegmentIntersectionTest(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, ref Vector2 point)
        {
            var s10_x = p1.x - p0.x;
            var s10_y = p1.y - p0.y;
            var s32_x = p3.x - p2.x;
            var s32_y = p3.y - p2.y;

            var denom = s10_x * s32_y - s32_x * s10_y;

            if (denom == 0) return false; // no collision

            var denom_is_positive = denom > 0;

            var s02_x = p0.x - p2.x;
            var s02_y = p0.y - p2.y;

            var s_numer = s10_x * s02_y - s10_y * s02_x;

            if ((s_numer < 0) == denom_is_positive) return false; // no collision

            var t_numer = s32_x * s02_y - s32_y * s02_x;

            if ((t_numer < 0) == denom_is_positive) return false; // no collision

            if ((s_numer > denom) == denom_is_positive || (t_numer > denom) == denom_is_positive) return false; // no collision

            // collision detected

            var t = (float)t_numer / denom;

            point.x = (int)(p0.x + (t * s10_x));
            point.y = (int)(p0.y + (t * s10_y));

            return true;
        }
    }
}
