using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mugen3D;

[ExecuteInEditMode]
public class CollideSystemTest : MonoBehaviour {
    public OBB cuboid1;
    public OBB cuboid2; 
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        
	}

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector3[] c1Points = cuboid1.GetVertexArray().ToArray();
        Vector3[] c2Points = cuboid2.GetVertexArray().ToArray();
        if (PhysicsUtils.CuboidCuboidTest(c1Points, c2Points))
        {
            Gizmos.color = Color.red;
        }
        DrawCuboid(c1Points);
        DrawCuboid(c2Points);
    }

    void DrawCuboid(Vector3[] points)
    {
        //下表面
        Mesh quad = CreateQuad(points[3], points[2], points[1], points[0]);
        Gizmos.DrawMesh(quad);
        //上表面
        quad = CreateQuad(points[4], points[5], points[6], points[7]);
        Gizmos.DrawMesh(quad);
        //左表面
        quad = CreateQuad(points[5], points[4], points[0], points[1]);
        Gizmos.DrawMesh(quad);
        //右表面
        quad = CreateQuad(points[7], points[6], points[2], points[3]);
        Gizmos.DrawMesh(quad);
        //前表面
        quad = CreateQuad(points[4], points[7], points[3], points[0]);
        Gizmos.DrawMesh(quad);
        //后表面
        quad = CreateQuad(points[6], points[5], points[1], points[2]);
        Gizmos.DrawMesh(quad);
    }

    Mesh CreateQuad(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
    {
        Vector3[] vertices = {
            p1,p2,p3,p4
        };
        int[] indices = new int[6] { 0, 1, 3, 1, 2, 3 };
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = indices;
        mesh.RecalculateNormals();
        return mesh;
    }
}
