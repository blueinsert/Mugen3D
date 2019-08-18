using System;
using System.Collections;
using System.Collections.Generic;
using FixPointMath;
using Math = FixPointMath.Math;

namespace bluebean.Mugen3D.Core
{
    //public delegate bool IntersectChecker<T1, T2>(T1 c1, T2 c2, out ContactInfo contactInfo) where T1 : Collider where T2 : Collider;
    public delegate bool IntersectChecker(Collider c1, Collider c2, out ContactInfo contactInfo);

    public class PhysicsUtils
    {
        /*
        private static Vector3 GetPlaneNormalVector(Vector3 p1, Vector3 p2, Vector3 p3)
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
         */
    
        public static bool RectRectTest(Rect rect1, Rect rect2)
        {
            bool isIntersect = false;
            if (Math.Abs(rect1.position.x - rect2.position.x) < (rect1.width + rect2.width) / 2 && Math.Abs(rect2.position.y - rect1.position.y) < (rect1.height + rect2.height) / 2)
                isIntersect = true;
            return isIntersect;
        }


        public static bool SegmentIntersectionTest(Vector p0, Vector p1, Vector p2, Vector p3, ref Vector point)
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

            var t = t_numer / denom;

            point.x = (int)(p0.x + (t * s10_x));
            point.y = (int)(p0.y + (t * s10_y));

            return true;
        }

        private static Dictionary<ColliderType, Dictionary<ColliderType, IntersectChecker>> intersectCheckers = new Dictionary<ColliderType, Dictionary<ColliderType, IntersectChecker>> {
            {ColliderType.ComplexCollider, new Dictionary<ColliderType, IntersectChecker>{ { ColliderType.ComplexCollider, ComplexColliderIntersectTest } } },
            {ColliderType.RectCollider, new Dictionary<ColliderType, IntersectChecker>{ { ColliderType.RectCollider, RectColliderIntersectTest } } },
        };

        public static bool RectColliderIntersectTest(Collider c1, Collider c2, out ContactInfo contactInfo)
        {
            var rc1 = c1 as RectCollider;
            var rc2 = c2 as RectCollider;
            contactInfo = null;
            bool intersect = !((rc1.xMin > rc2.xMax || rc2.xMin > rc1.xMax) || (rc1.yMin > rc2.yMax || rc2.yMin > rc1.yMax));
            if (intersect)
            {
                Vector dir = new Vector(rc1.m_position.x > rc2.m_position.x ? 1 : -1, 0);
                Number depth = (rc1.m_width + rc2.m_width) / 2 - Math.Abs(rc1.m_position.x - rc2.m_position.x);
                contactInfo = new ContactInfo() { recoverDir = dir, depth = depth};
                return true;
            }
            return intersect;
        }

        private static bool ComplexColliderIntersectTest(Collider c1, Collider c2, out ContactInfo contactInfo)
        {
            var cc1 = c1 as ComplexCollider;
            var cc2 = c2 as ComplexCollider;
            contactInfo = null;
            for (int i = 0; i < cc1.m_collideClsnsLength; i++)
            {
                var rect1 = cc1.collideClsns[i];
                for (int j = 0; j < cc2.m_collideClsnsLength; j++)
                {
                    var rect2 = cc2.collideClsns[j];
                    if(RectColliderIntersectTest(rect1, rect2, out contactInfo)) {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool IsIntersect(Collider c1, Collider c2, out ContactInfo contactInfo)
        {
            IntersectChecker checker = intersectCheckers[c1.type][c2.type];
            return checker(c1, c2, out contactInfo);
        }


     
    }
}
