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

    // Start is called before the first frame update
    void Start()
    {
        //instantiate all wagons
        InstantiateWagons();
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
            Debug.Log(positions[wagons.Count]);
            wagons.Add(wagon);
        }
        //instantiate compartmentWagons, these will have added walls and benches
        for (int i = 0; i < compartmentWagons; i++)
        {
            GameObject wagon = PhotonNetwork.Instantiate(wagonPrefab.name, positions[wagons.Count], correctRotation);
            wagon.transform.parent = this.transform;
            GameObject compartment = PhotonNetwork.Instantiate(compartmentPrefab.name, positions[wagons.Count], correctRotation);
            compartment.transform.parent = wagon.transform;
            Debug.Log(positions[wagons.Count]);
            wagons.Add(wagon);
        }

        //instantiate diningWagons, these will have added tables and chairs
        for (int i = 0; i < diningWagons; i++)
        {
            GameObject wagon = PhotonNetwork.Instantiate(wagonPrefab.name, positions[wagons.Count], correctRotation);
            wagon.transform.parent = this.transform;
            GameObject diningGroup = PhotonNetwork.Instantiate(diningPrefab.name, positions[wagons.Count], correctRotation);
            diningGroup.transform.parent = wagon.transform;
            Debug.Log(positions[wagons.Count]);
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


  
}
