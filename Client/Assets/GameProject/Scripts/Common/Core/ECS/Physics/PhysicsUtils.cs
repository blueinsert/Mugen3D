using System;
using System.Collections;
using System.Collections.Generic;
using FixPointMath;
using Math = FixPointMath.Math;

namespace bluebean.Mugen3D.Core
{
    public class ContactInfo
    {
        public Vector recoverDir;
        public Number depth;
    }

    public class PhysicsUtils
    {
        
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


        public static bool RectColliderIntersectTest(RectCollider rc1, RectCollider rc2, out ContactInfo contactInfo)
        {
            contactInfo = null;
            bool intersect = !((rc1.xMin > rc2.xMax || rc2.xMin > rc1.xMax) || (rc1.yMin > rc2.yMax || rc2.yMin > rc1.yMax));
            if (intersect)
            {
                Vector dir = new Vector(rc1.Position.x > rc2.Position.x ? 1 : -1, 0);
                Number depth = (rc1.Width + rc2.Width) / 2 - Math.Abs(rc1.Position.x - rc2.Position.x);
                contactInfo = new ContactInfo() { recoverDir = dir, depth = depth};
                return true;
            }
            return intersect;
        }

        public static bool ComplexColliderIntersectTest(ComplexCollider cc1, ComplexCollider cc2, out ContactInfo contactInfo)
        {
            contactInfo = null;
            for (int i = 0; i < cc1.CollideClsnsLength; i++)
            {
                var rect1 = cc1.CollideClsns[i];
                for (int j = 0; j < cc2.CollideClsnsLength; j++)
                {
                    var rect2 = cc2.CollideClsns[j];
                    if(RectColliderIntersectTest(rect1, rect2, out contactInfo)) {
                        return true;
                    }
                }
            }
            return false;
        }

    }
}
