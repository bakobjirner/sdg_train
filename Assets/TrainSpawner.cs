using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainSpawner : MonoBehaviourPun
{
    public GameObject wagonPrefab;
    public GameObject compartmentPrefab;
    public GameObject diningPrefab;
    public GameObject gamingPrefab;

    public int compartmentWagons;
    public int diningWagons;
    public int cargoWagons;
    public int gameWagons;

    public float wagonSpacing;
    public float wagonHeight = 1;
    public float locomotiveSpacing;

    private List<GameObject> wagons;
    private List<Vector3> positions;
    private List<Vector3> spawnLocations;

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            //instantiate all wagons
            InstantiateWagons();
        }
    }


   private void InstantiateWagons()
    {
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
            photonView.RPC("addSpawnLocationsCargo", RpcTarget.AllBuffered, new object[]{positions[wagons.Count]});
            wagons.Add(wagon);
        }
        //instantiate compartmentWagons, these will have added walls and benches
        for (int i = 0; i < compartmentWagons; i++)
        {
            GameObject wagon = PhotonNetwork.Instantiate(wagonPrefab.name, positions[wagons.Count], correctRotation);
            wagon.transform.parent = this.transform;
            GameObject compartment = PhotonNetwork.Instantiate(compartmentPrefab.name, positions[wagons.Count], correctRotation);
            compartment.transform.parent = wagon.transform;
            photonView.RPC("addSpawnLocationsCompartment", RpcTarget.AllBuffered, new object[] {positions[wagons.Count] });
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

        //instantiate gaming wagons, these will have added arcades
        for (int i = 0; i < gameWagons; i++)
        {
            GameObject wagon = PhotonNetwork.Instantiate(wagonPrefab.name, positions[wagons.Count], correctRotation);
            wagon.transform.parent = this.transform;
            GameObject gameGroup = PhotonNetwork.Instantiate(gamingPrefab.name, positions[wagons.Count], correctRotation);
            gameGroup.transform.parent = wagon.transform;
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
        int totalWagons = diningWagons + compartmentWagons + cargoWagons + gameWagons;
        positions = new List<Vector3>();
        for(int i = 0; i < totalWagons; i++) {
            positions.Add(new Vector3(0, wagonHeight, wagonSpacing * i + locomotiveSpacing));
        }
        positions = Shuffle(positions);
    }

    /**
     * adds spawnlocations relative to wagon position
     */
    [PunRPC]
    private List<Vector3> addSpawnLocationsCargo(Vector3 relativePosition)
    {
        if (spawnLocations == null)
        {
            spawnLocations = new List<Vector3>();
        }

        //in cargo, just spawn in the middle
        spawnLocations.Add(relativePosition);
        Debug.Log("addedSpawn " + spawnLocations.Count);
        return spawnLocations;
    }

    /**
     * adds spawnlocations relative to wagon position
     */
    [PunRPC]
    private List<Vector3> addSpawnLocationsCompartment(Vector3 relativePosition)
    {
       
        if (spawnLocations == null)
        {
            spawnLocations = new List<Vector3>();
        }
        //in compartmentwagon, spawn in one of the compartments
        spawnLocations.Add(new Vector3(-1,0,-7.5f) + relativePosition);
        spawnLocations.Add(new Vector3(-1,0, -4.5f) + relativePosition);
        spawnLocations.Add(new Vector3(-1,0, -1.5f) + relativePosition);
        spawnLocations.Add(new Vector3(-1,0, 1.5f) + relativePosition);
        spawnLocations.Add(new Vector3(-1,0, 4.5f) + relativePosition);
        spawnLocations.Add(new Vector3(-1,0, 7.5f) + relativePosition);
        Debug.Log("addedSpawn " + spawnLocations.Count);
        return spawnLocations;
    }


    public List<Vector3> getSpawnLocations()
    {
        if (spawnLocations != null && spawnLocations.Count > 0)
        {
            return spawnLocations;
        }
        List<Vector3> list = new List<Vector3>();
        list.Add(Vector3.zero);
        return list;
    }
}
