using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ProceduralCube : MonoBehaviour
{


    public static Vector3[] GetVertices(float x, float y, float z, Vector3 position)
    {
        Vector3[] vertices = new Vector3[]
        {

            //Front
            new Vector3(0,0,0) + position,
            new Vector3(x,0,0) + position,
            new Vector3(x,y,0) + position,
            new Vector3(0,y,0) + position,

            //Back
            new Vector3(0,0,z) + position,
            new Vector3(x,0,z) + position,
            new Vector3(x,y,z) + position,
            new Vector3(0,y,z) + position,
        };

        return vertices;
    }


    /**
     * use an offset for previously generated vertices 
     */
    public static  int[] GetTriangles(int offset)
    {
        int[] triangles = new int[]
        {
            //Front (0,1,2,3)
            2+offset, 1+offset, 0+offset,
            3+offset, 2+offset, 0+offset,

            //Back (4,5,6,7)
            4+offset,5+offset,6+offset,
            6+offset,7+offset,4+offset,

            //Left (0,3,4,7)
            3+offset,0+offset,4+offset,
            4+offset,7+offset,3+offset,

            //Right (1,2,5,6)
            1+offset,2+offset,6+offset,
            1+offset,6+offset,5+offset,

            //Top (2,3,6,7)
            2+offset,3+offset,7+offset,
            2+offset,7+offset,6+offset,

            //Bottom (0,1,4,5)
            0+offset,1+offset,4+offset,
            4+offset,1+offset,5+offset

        };

        return triangles;
    }


    public static Vector3[] GetNormals()
    {
        Vector3[] normals = new Vector3[]
        {
            //Front
            new Vector3(0,0,-1),
            new Vector3(0,0,-1),
            new Vector3(0,0,-1),
            new Vector3(0,0,-1),

            //Back
            new Vector3(0,0,1),
            new Vector3(0,0,1),
            new Vector3(0,0,1),
            new Vector3(0,0,1),

        };

        return normals;
    }


    public static Vector2[] GetUVs()
    {
        Vector2[] uvs = new Vector2[]
        {
             // Front
        new Vector2(0, 0),
        new Vector2(1, 0),
        new Vector2(1, 1),
        new Vector2(0, 1),

        // Back
        new Vector2(1, 0),
        new Vector2(0, 0),
        new Vector2(0, 1),
        new Vector2(1, 1),

    };
        return uvs;
    }
}
