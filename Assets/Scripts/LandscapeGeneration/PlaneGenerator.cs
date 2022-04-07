using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class PlaneGenerator : MonoBehaviour
{

    //create a basic plane
    public static Vector3[] GetVertices(int resolutionX, int resolutionZ, float scaleX, float scaleZ)
    {
        Vector3[] vertices;
        vertices = new Vector3[(resolutionX + 1) * (resolutionZ + 1)];
        //set verticePositions
        int index = 0;
        for (int x = 0; x <= resolutionX; x++)
        {
            for (int z = 0; z <= resolutionZ; z++)
            {
                vertices[index] = new Vector3(x*(scaleX/resolutionX)- scaleX/2, 0, z*(scaleZ/resolutionZ) - scaleZ / 2);
                index++;
            }
        }
        return vertices;
    }

    public static int[] GetTriangles(int resolutionX, int resolutionZ)
    {
        int[] triangles;

        //create one quad cosisting out of two triangles per loop
        int quad = 0;
        int tris = 0;
        triangles = new int[resolutionX * resolutionZ * 6];
        for (int x = 0; x < resolutionX; x++)
        {
            for (int z = 0; z < resolutionZ; z++)
        {
            
                triangles[0 + tris] = quad + 0;
                triangles[2 + tris] = quad + resolutionZ + 1;
                triangles[1 + tris] = quad + 1;
                triangles[5 + tris] = quad + resolutionZ + 1;
                triangles[4 + tris] = quad + resolutionZ + 2;
                triangles[3 + tris] = quad + 1;
                quad++;
                tris += 6;
            }
            //prevent creation of triangles between rows
            quad++;
       }

        return triangles;

    }

    public static Vector2[] GetUvs(Vector3[] vertices, int resolutionX, int resolutionZ)
    {
        Vector2[] uvs = new Vector2[vertices.Length];
        int index = 0;
        for (int x = 0; x <= resolutionX; x++)
        {
            for (int z = 0; z <= resolutionZ; z++)
            {
                uvs[index] = new Vector2(x * 1.0f / resolutionX, z * 1.0f / resolutionZ);
                index++;
            }
        }
        return uvs;
    }
    
}
