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

    public GameObject tracks;
    public GameObject rails;

    //the size of the world in each direction
    public float worldDimension = 10;
    //the resolution of the world
    public int worldResolution = 10;
    public Material material;
    public Material trackMaterial;
    public float scrollSpeed = 1;
    public float hillSize = 1;
    public float noiseZoom = 1;

    [Range(0, 1)]
    public float trenchWidth = .5f;
    [Range(0, 1)]
    public float trenchSteepness = .5f;

    public float trackWidth = 2;
    public int trackResolution = 1000;
    public int numberOfTies = 200;
    public float tieHeight = 0.1f;
    public int tieDistance = 5;
    public Color tieColor;

    [Range(0, 1)]
    public float railDistance = .5f;
    public float railWidth = .1f;
    public float railHeight = .1f;

    public float trackSpeedFactor = 100;

    private void setValues()
    {
        meshFilter = this.GetComponent<MeshFilter>();
        mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        meshFilter.mesh = mesh;
        CreatePlane();
        UpdateMesh();
        material.SetFloat("_scrollSpeed", scrollSpeed);
        material.SetFloat("_hillSize", hillSize);
        material.SetFloat("_trenchSteepness", trenchSteepness);
        material.SetFloat("_trenchWidth", trenchWidth);
        material.SetFloat("_noiseZoom", noiseZoom);
    }

   


    private void OnValidate()
    {
        setValues();
        SetTracks();
        SetRails();
    }


    //create a basic plane
    private void CreatePlane()
    {
        vertices = PlaneGenerator.GetVertices(worldResolution, worldResolution, worldDimension, worldDimension);
        uvs = PlaneGenerator.GetUvs(vertices, worldResolution, worldResolution);
        triangles = PlaneGenerator.GetTriangles(worldResolution, worldResolution);
    }

  

    private void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
    }

    private void SetTracks()
    {
        Vector3[] vertices;
        int[] triangles;
        Vector2[] uvs;
        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        MeshFilter meshFilter = tracks.GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        vertices = PlaneGenerator.GetVertices(8, trackResolution, trackWidth, worldDimension);
        uvs = PlaneGenerator.GetUvs(vertices, 8, trackResolution);
        triangles = PlaneGenerator.GetTriangles(8, trackResolution);
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        trackMaterial.SetFloat("_numberOfTies", numberOfTies);
        trackMaterial.SetFloat("_speed", scrollSpeed* trackSpeedFactor);
        trackMaterial.SetFloat("_tieHeight", tieHeight);
        trackMaterial.SetColor("_BaseColor", tieColor);
        trackMaterial.SetFloat("_tieDistance", (float)tieDistance);
    }

    private void SetRails()
    {
        Vector3[] verticesCombined;
        Vector3[] verticesRight;
        Vector3[] verticesLeft;
        int[] trianglesLeft;
        int[] trianglesRight;
        int[] trianglesCombined;
        Vector2[] uvCombined;
        Vector2[] uvLeft;
        Vector2[] uvRight;
        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        MeshFilter meshFilter = rails.GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        verticesRight = ProceduralCube.GetVertices(railWidth, railHeight, worldDimension, new Vector3(trackWidth*railDistance/2, tieHeight, -worldDimension/2));
        trianglesRight = ProceduralCube.GetTriangles(0);
        verticesLeft = ProceduralCube.GetVertices(railWidth, railHeight, worldDimension, new Vector3(-trackWidth * railDistance / 2, tieHeight, -worldDimension / 2));
        trianglesLeft = ProceduralCube.GetTriangles(8);
        uvLeft = ProceduralCube.GetUVs();
        uvRight = uvLeft;

        verticesCombined = new Vector3[verticesLeft.Length + verticesRight.Length];
        verticesLeft.CopyTo(verticesCombined, 0);
        verticesRight.CopyTo(verticesCombined, verticesLeft.Length);
        trianglesCombined = new int[trianglesLeft.Length+trianglesRight.Length];
        trianglesLeft.CopyTo(trianglesCombined, 0);
        trianglesRight.CopyTo(trianglesCombined, trianglesLeft.Length);
        uvCombined = new Vector2[uvLeft.Length + uvRight.Length];
        uvLeft.CopyTo(uvCombined, 0);
        uvRight.CopyTo(uvCombined, uvLeft.Length);
        mesh.Clear();
        mesh.vertices = verticesCombined;
        mesh.triangles = trianglesCombined;
        mesh.uv = uvCombined;
        mesh.RecalculateNormals();
    }
}
