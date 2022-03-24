using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class LandscapeGenerator : MonoBehaviour
{

    Vector3[] vertices;
    int[] triangles;
    Mesh mesh;
    MeshFilter meshFilter;
    //the size of the world in each direction
    public int worldDimension = 10;
    //the resolution of the world
    public int worldResolution = 10;

    // Start is called before the first frame update
    void Start()
    {
        meshFilter = this.GetComponent<MeshFilter>();
        mesh = new Mesh();
        meshFilter.mesh = mesh;

        CreatePlane();
        CreateTriangles();
        UpdateMesh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //create a basic plane
    private void CreatePlane()
    {
       
        //by default the world is square
       
        vertices = new Vector3[worldDimension*worldResolution* worldDimension * worldResolution];
        //set verticePositions
        int index = 0;
        for(int x = 0; x< worldDimension * worldResolution; x++)
        {
            for(int z = 0; z < worldDimension * worldResolution; z++)
            {
                float gridscale = 1f / worldResolution;
                vertices[index] = new Vector3(x*gridscale, 0, z*gridscale);
                index++;
                
            }
        }
    }

    private void CreateTriangles()
    {
        

        triangles = new int[3];
        triangles[0] = 0;
        triangles[1] = worldDimension * worldResolution + 1;
        triangles[2] = 1;
        

    }

    private void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    private void OnDrawGizmos()
    {
        for(int i = 0; i< vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], .1f);
        }
    }
}
