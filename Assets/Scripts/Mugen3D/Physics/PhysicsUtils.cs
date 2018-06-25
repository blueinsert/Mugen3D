﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D
{
    public class PhysicsUtils
    {
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

        public static bool GeometryOverlapTest(Geometry g1, Geometry g2)
        {
            return false;
        }

        public static bool RectRectTest(Rect rect1, Rect rect2)
        {
            bool isIntersect = false;
            if (Mathf.Abs(rect1.position.x - rect2.position.x) < (rect1.width + rect2.width) / 2 && Mathf.Abs(rect2.position.y - rect1.position.y) < (rect1.height + rect2.height) / 2)
                isIntersect = true;
            return isIntersect;
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

        /*
        public static void ClosestPointAtOBB(OBB cuboid, Vector3 p, out Vector3 q){
            q = Vector3.zero;
            Matrix4x4 trsM = cuboid.TransformMatrix;    
            Vector3 localXAxis = trsM.GetColumn(0);
            Vector3 localYAxis = trsM.GetColumn(1);
            Vector3 localZAxis = trsM.GetColumn(2);
            float localX = Vector3.Dot(p, localXAxis) / (localXAxis.magnitude * localXAxis.magnitude);
            float localY = Vector3.Dot(p, localYAxis) / (localYAxis.magnitude * localYAxis.magnitude);
            float localZ = Vector3.Dot(p, localZAxis) / (localZAxis.magnitude * localZAxis.magnitude);
            Vector3 pLocal = new Vector3(localX, localY, localZ);
            for (int i = 0; i < 3; i++)
            {
                float v = pLocal[i];
                v = Mathf.Max(v, -0.5f);
                v = Mathf.Min(v, 0.5f);
                q[i] = v;
            }
            q = trsM * q;
        }

        public static float DistToOBB(OBB cuboid, Vector3 p)
        {
            Vector3 closestPoint;
            ClosestPointAtOBB(cuboid, p, out closestPoint);
            Vector3 center = cuboid.GetCenter();
            return (p - center).magnitude - (closestPoint - center).magnitude;
        }

        public static float DistToGeometry(Geometry geometry, Vector3 p)
        {
            if (geometry is OBB)
            {
                return DistToOBB(geometry as OBB, p);
            }
            else
            {
                return 0;
            }
        }
        */

        private static Vector3 LocalPoint(Matrix4x4 modelMatrix, Vector3 globalPoint)
        {
            Vector3 localXAxis = modelMatrix.GetColumn(0);
            Vector3 localYAxis = modelMatrix.GetColumn(1);
            Vector3 localZAxis = modelMatrix.GetColumn(2);
            Vector3 modelOrigin = modelMatrix.GetColumn(3);
            Vector3 v = globalPoint - modelOrigin;
            float localX = Vector3.Dot(v, localXAxis) / (localXAxis.magnitude * localXAxis.magnitude);
            float localY = Vector3.Dot(v, localYAxis) / (localYAxis.magnitude * localYAxis.magnitude);
            float localZ = Vector3.Dot(v, localZAxis) / (localZAxis.magnitude * localZAxis.magnitude);
            Vector3 pLocal = new Vector3(localX, localY, localZ);
            return pLocal;
        }

        private static bool RayUnitCubeIntersectTest(Vector3 rayStart, Vector3 rayDir, float rayLength, out float dist, out Vector3 p, out Vector3 normal)
        {
            dist = 0;
            normal = Vector3.zero;
            p = rayStart;
            if(rayStart.x <= 0.5 && rayStart.x >= -0.5 && 
               rayStart.y <= 0.5 && rayStart.y >= -0.5 && 
               rayStart.z <= 0.5 && rayStart.z >= -0.5){
                return true;
            }
            float tmin = 0;
            float tmax = float.MaxValue;
            for (int i = 0; i < 3; i++)
            {
                if (Mathf.Abs(rayDir[i]) < 0.001)
                {
                    if (rayStart[i] < -0.5 || rayStart[i] > 0.5)
                        return false;
                }
                else
                {
                    float ood = 1.0f / rayDir[i];
                    float t1 = (0.5f - rayStart[i]) * ood;
                    float t2 = (-0.5f - rayStart[i]) * ood;
                    if (t1 > t2)
                    {
                        float tmp = t1;
                        t1 = t2;
                        t2 = tmp;
                    }
                    if (t1 > tmin)
                        tmin = t1;
                    if (t2 < tmax)
                        tmax = t2;
                    if (tmin > tmax)
                        return false;
                }
            }
            if (tmin > rayLength)
                return false;
            p = tmin * rayDir + rayStart;
            for (int i = 0; i < 3; i++)
            {
                if (p[i] == 0.5)
                    normal[i] = 1;
                else if (p[i] == -0.5)
                    normal[i] = -1;
                else
                    normal[i] = 0;
            }
            normal.Normalize();
            dist = tmin;
            return true;
        }

      
    }
}