using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class RailGenerator : MonoBehaviour
{

    Vector3[] vertices;
    int[] triangles;
    Mesh mesh;
    MeshFilter meshFilter;
    public float railWidth = .1f;
    public float railHeight = .5f;
    public float railLength = 200;
    public float trackWidth = 2;
    public float trackHeigth = .5f;
    public float tieWidth = 2.5f;
    public float tieHeigth = .2f;
    public float tieLength = .5f;
    public float tieDistance = 1;

    public void OnValidate()
    {
        CreateRails();
    }

    private void CreateRails()
    {
        meshFilter = this.GetComponent<MeshFilter>();
        mesh = new Mesh();
        meshFilter.mesh = mesh;

        //railOne
        Vector3[] verticesRailOne = ProceduralCube.GetVertices(railWidth, railHeight, railLength, new Vector3(trackWidth, trackHeigth, -railLength / 2));
        int[] trianglesRailOne = ProceduralCube.GetTriangles(0);

        //railTwo
        Vector3[] verticesRailTwo = ProceduralCube.GetVertices(railWidth, railHeight, railLength, new Vector3(-trackWidth, trackHeigth, -railLength / 2));
        int[] trianglesRailTwo = ProceduralCube.GetTriangles(8);

        vertices = new Vector3[verticesRailOne.Length + verticesRailTwo.Length];
        triangles = new int[trianglesRailOne.Length + trianglesRailTwo.Length];

        verticesRailOne.CopyTo(vertices, 0);
        verticesRailTwo.CopyTo(vertices, verticesRailOne.Length);
        
        trianglesRailOne.CopyTo(triangles, 0);
        trianglesRailTwo.CopyTo(triangles, trianglesRailOne.Length);


        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.name = "rail";
        meshFilter.mesh = mesh;
        mesh.RecalculateNormals();
    }
}
