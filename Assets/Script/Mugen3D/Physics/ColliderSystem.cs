using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D
{
    public class ColliderSystem
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

        public static bool RectRectTest(Box2D rect1, Box2D rect2)
        {
            bool isIntersect = false;
            float r1xmin = rect1.center.x - rect1.width / 2;
            float r1xmax = rect1.center.x + rect1.width / 2;
            float r1ymin = rect1.center.y - rect1.height / 2;
            float r1ymax = rect1.center.y + rect1.height / 2;

            float r2xmin = rect2.center.x - rect2.width / 2;
            float r2xmax = rect2.center.x + rect2.width / 2;
            float r2ymin = rect2.center.y - rect2.height / 2;
            float r2ymax = rect2.center.y + rect2.height / 2;
            if (r1xmax < r2xmin || r1xmin > r2xmax || r1ymax < r2ymin || r1ymin > r2ymax)
            {
                isIntersect = false;
            }
            else
            {
                isIntersect = true;
            }
            return isIntersect;
        }
    }
}
