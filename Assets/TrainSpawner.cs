using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainSpawner : MonoBehaviour
{
    public GameObject wagonPrefab;
    public GameObject compartmentPrefab;
    public GameObject diningPrefab;

    public int compartmentWagons;
    public int diningWagons;
    public int cargoWagons;

    public float wagonSpacing;
    public float wagonHeight = 1;
    public float locomotiveSpacing;

    private List<GameObject> wagons;
    private List<Vector3> positions;
    private List<Vector3> spawnLocations;

    // Start is called before the first frame update
    void Start()
    {
        //instantiate all wagons
        InstantiateWagons();
    }


   private void InstantiateWagons()
    {
        spawnLocations = new List<Vector3>();
        //generate random positions
        GeneratePositions();
        //set the correct rotation
        Quaternion correctRotation = Quaternion.Euler(-90, 0, 0);

        wagons = new List<GameObject>();
       
        //instantiate cargowagons
        for(int i = 0; i < cargoWagons; i++)
        {
            GameObject wagon = PhotonNetwork.Instantiate(wagonPrefab.name, positions[wagons.Count], correctRotation);
            wagon.transform.parent = this.transform;
            spawnLocations = addSpawnLocationsCargo(spawnLocations, positions[wagons.Count]);
            wagons.Add(wagon);
        }
        //instantiate compartmentWagons, these will have added walls and benches
        for (int i = 0; i < compartmentWagons; i++)
        {
            GameObject wagon = PhotonNetwork.Instantiate(wagonPrefab.name, positions[wagons.Count], correctRotation);
            wagon.transform.parent = this.transform;
            GameObject compartment = PhotonNetwork.Instantiate(compartmentPrefab.name, positions[wagons.Count], correctRotation);
            compartment.transform.parent = wagon.transform;
            spawnLocations = addSpawnLocationsCompartment(spawnLocations, positions[wagons.Count]);
            wagons.Add(wagon);
        }

        //instantiate diningWagons, these will have added tables and chairs
        for (int i = 0; i < diningWagons; i++)
        {
            GameObject wagon = PhotonNetwork.Instantiate(wagonPrefab.name, positions[wagons.Count], correctRotation);
            wagon.transform.parent = this.transform;
            GameObject diningGroup = PhotonNetwork.Instantiate(diningPrefab.name, positions[wagons.Count], correctRotation);
            diningGroup.transform.parent = wagon.transform;
            wagons.Add(wagon);
        }
    }

    private List<Vector3> Shuffle(List<Vector3> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int k = Random.Range(0, list.Count);
            Vector3 value = list[k];
            list[k] = list[i];
            list[i] = value;
        }
        return list;
    }

    private void GeneratePositions()
    {
        int totalWagons = diningWagons + compartmentWagons + cargoWagons;
        positions = new List<Vector3>();
        for(int i = 0; i < totalWagons; i++) {
            positions.Add(new Vector3(0, wagonHeight, wagonSpacing * i + locomotiveSpacing));
        }
        positions = Shuffle(positions);
    }

    /**
     * adds spawnlocations relative to wagon position
     */
    private List<Vector3> addSpawnLocationsCargo(List<Vector3> spawnLocations, Vector3 relativePosition)
    {
        //in cargo, just spawn in the middle
        spawnLocations.Add(relativePosition);
        return spawnLocations;
    }

    /**
     * adds spawnlocations relative to wagon position
     */
    private List<Vector3> addSpawnLocationsCompartment(List<Vector3> spawnLocations, Vector3 relativePosition)
    {
        //in compartmentwagon, spawn in one of the compartments
        spawnLocations.Add(new Vector3(-1,0,-7.5f) + relativePosition);
        spawnLocations.Add(new Vector3(-1,0, -4.5f) + relativePosition);
        spawnLocations.Add(new Vector3(-1,0, -1.5f) + relativePosition);
        spawnLocations.Add(new Vector3(-1,0, 1.5f) + relativePosition);
        spawnLocations.Add(new Vector3(-1,0, 4.5f) + relativePosition);
        spawnLocations.Add(new Vector3(-1,0, 7.5f) + relativePosition);
        return spawnLocations;
    }


    public List<Vector3> getSpawnLocations()
    {
        if (spawnLocations.Count > 0)
        {
            return spawnLocations;
        }
        List<Vector3> list = new List<Vector3>();
        list.Add(Vector3.zero);
        return list;
    }
}
