using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class LandscapeGeneratorShader : MonoBehaviour
{
    Vector3[] vertices;
    int[] triangles;
    Vector2[] uvs;
    Mesh mesh;
    MeshFilter meshFilter;
    //the size of the world in each direction
    public int worldDimension = 10;
    //the resolution of the world
    public int worldResolution = 10;


    //Object to follow
    public Transform objectToFollow;

    // Start is called before the first frame update
    void Start()
    {
        meshFilter = this.GetComponent<MeshFilter>();
        mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        meshFilter.mesh = mesh;

        CreatePlane();
        CreateTriangles();
        UpdateMesh();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = new Vector3(objectToFollow.position.x,0,objectToFollow.position.z);
    }


    //create a basic plane
    private void CreatePlane()
    {

        vertices = new Vector3[(worldResolution + 1) * (worldResolution + 1)];
        uvs = new Vector2[vertices.Length];
        //set verticePositions
        int index = 0;
        for (int x = 0; x <= worldResolution; x++)
        {
            for (int z = 0; z <= worldResolution; z++)
            {
                float gridscale = worldDimension / worldResolution;
                vertices[index] = new Vector3(x * gridscale - 0.5f * worldDimension, 0, z * gridscale - 0.5f * worldDimension);
                uvs[index] = new Vector2(x*1.0f / worldResolution, z * 1.0f / worldResolution);
                index++;
                
            }
        }
    }

    private void CreateTriangles()
    {


        //create one quad cosisting out of two triangles per loop
        int quad = 0;
        int tris = 0;
        triangles = new int[worldResolution * worldResolution * 6];
        for (int z = 0; z < worldResolution; z++)
        {
            for (int x = 0; x < worldResolution; x++)
            {
                triangles[0 + tris] = quad + 0;
                triangles[1 + tris] = quad + 1;
                triangles[2 + tris] = quad + worldResolution + 1;
                triangles[3 + tris] = quad + 1;
                triangles[4 + tris] = quad + worldResolution + 2;
                triangles[5 + tris] = quad + worldResolution + 1;
                quad++;
                tris += 6;
            }
            //prevent creation of triangles between rows
            quad++;
        }

    }

    private void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
    }
}
